using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BatRobot : MonoBehaviour
{
    [Header("Idle")]
    [SerializeField] private Vector2 bounds;
    [SerializeField] private float reachedTargetDistance;

    [SerializeField] private float targetPosLerpSpeed;
    [SerializeField] private float targetPosLerpSpeedChange;
    [SerializeField] private float angleChangeSpeed;
    [SerializeField] private float xAmplitude;
    [SerializeField] private float yAmplitude;

    [SerializeField] private float maxRollAngle;

    [SerializeField] private LayerMask playerDetectLayers;

    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Collider2D[] colliders;

    [Header("Attack")]
    [SerializeField] private float playerDetectAngle;
    [SerializeField] private float attackFollowDuration;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform barrel;
    [SerializeField] private Transform shootTransform;
    [SerializeField] private float shootInterval;

    [Header("Sleep")]
    [SerializeField] private float sleepDuration;
    [SerializeField] private float wakeUpDuration;

    [Header("Visual")]
    [SerializeField] private Transform attackingIndicatorTransform;
    [SerializeField] private float attackIndicatorActivePos;
    [SerializeField] private Animator animator;


    private Transform player;

    private float xOffset;
    private float yOffset;

    private float angle;
    private float currentTargetPosLerpSpeed;

    private Vector3 startPoint;
    private Vector3 currentPoint;
    private Vector3 targetPoint;
    private Vector3 totalPoint;

    private float remainingAttackDuration;
    private float nextAttackDuration;

    private BatRobotState batRobotState;

    private bool isSleeping;

    private void Start()
    {
        player = FindObjectOfType<CharacterController>().transform;

        startPoint = transform.position;

        currentPoint = startPoint;

        SetNewtargetPoint();
    }

    private void FixedUpdate()
    {
        if (batRobotState != BatRobotState.Sleep)
        {
            LookForPlayer();

            xOffset = Mathf.Cos(angle);
            yOffset = Mathf.Sin(angle);

            Vector3 moveDir = targetPoint - currentPoint;

            if (moveDir.sqrMagnitude < (reachedTargetDistance * reachedTargetDistance))
            {
                SetNewtargetPoint();
            }
            else
            {
                currentPoint += moveDir.normalized * currentTargetPosLerpSpeed * Time.deltaTime;
            }

            totalPoint = currentPoint + new Vector3(xOffset * xAmplitude, yOffset * yAmplitude, 0);

            transform.position = totalPoint;

            float _angle = (yOffset > 0) ? xOffset.Remap(-1, 1, -maxRollAngle, maxRollAngle) : xOffset.Remap(-1, 1, -maxRollAngle, maxRollAngle);

            transform.rotation = Quaternion.Euler(0, 0, _angle);

            currentTargetPosLerpSpeed += targetPosLerpSpeedChange * Time.deltaTime;
            currentTargetPosLerpSpeed = Mathf.Min(currentTargetPosLerpSpeed, targetPosLerpSpeed);

            angle += angleChangeSpeed * Time.deltaTime;

            if (batRobotState == BatRobotState.Attack)
            {
                remainingAttackDuration -= Time.deltaTime;

                nextAttackDuration -= Time.deltaTime;

                Vector2 dir = ((player.position + Vector3.up) - shootTransform.position).normalized;

                float rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                barrel.rotation = Quaternion.Euler(0, 0, rotation + 90);

                Shoot(dir);
            }
            else
            {
                barrel.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void LookForPlayer()
    {
        if (player != null)
        {
            Vector2 rayDir = (Vector2)player.position - (Vector2)(transform.position);
            float dirMagnitude = rayDir.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir.normalized, dirMagnitude, playerDetectLayers);

            Debug.DrawRay(transform.position, rayDir * dirMagnitude, Color.blue);
            if (hit.collider != null)
            {
                //Debug.Log(gameObject.name + " found " + hit.collider.gameObject.name);

                float angle = Vector2.Angle(Vector3.down, rayDir);

                if (hit.collider.CompareTag("Player") && angle < playerDetectAngle)
                {
                    if (batRobotState != BatRobotState.Attack)
                    {
                        ToggleAttackIndicator(true);
                    }

                    batRobotState = BatRobotState.Attack;
                    remainingAttackDuration = attackFollowDuration;
                }
                else if (remainingAttackDuration <= 0)
                {
                    if (batRobotState != BatRobotState.Idle)
                    {
                        ToggleAttackIndicator(false);
                    }

                    batRobotState = BatRobotState.Idle;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Boomerang") && !isSleeping)
        {
            Sleep();
        }
    }

    private void Sleep()
    {
        batRobotState = BatRobotState.Sleep;

        isSleeping = true;

        if (batRobotState == BatRobotState.Attack)
        {
            ToggleAttackIndicator(false);
        }

        animator.enabled = false;

        rigidbody.bodyType = RigidbodyType2D.Dynamic;

        StartCoroutine(WakeUpRoutine());
    }

    private void WakeUp()
    {
        isSleeping = false;

        animator.enabled = true;

        batRobotState = BatRobotState.Idle;
    }

    private IEnumerator WakeUpRoutine()
    {
        yield return new WaitForSeconds(sleepDuration);

        if (gameObject != null)
        {
            rigidbody.bodyType = RigidbodyType2D.Kinematic;

            foreach (var item in colliders)
            {
                item.enabled = false;
            }

            float t = 0;

            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            while (t <= 1)
            {
                t += Time.deltaTime / wakeUpDuration;

                transform.position = Vector3.Lerp(pos, totalPoint, t);

                transform.rotation = Quaternion.Lerp(rot, Quaternion.identity, t);

                yield return new WaitForEndOfFrame();
            }

            WakeUp();

            foreach (var item in colliders)
            {
                item.enabled = true;
            }
        }
    }

    private void Shoot(Vector2 dir)
    {
        if (nextAttackDuration <= 0)
        {
            nextAttackDuration = shootInterval;

            var instance = Instantiate(projectilePrefab);

            instance.transform.position = shootTransform.position;

            instance.Set(dir);
        }
    }

    private void SetNewtargetPoint()
    {
        currentTargetPosLerpSpeed = 0;
        targetPoint = startPoint + new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y));
    }

    private void ToggleAttackIndicator(bool activate)
    {
        float posY = activate ? attackIndicatorActivePos : 0;

        attackingIndicatorTransform.DOLocalMoveY(posY, .2f)
            .SetEase(Ease.InOutSine);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Application.isPlaying ? startPoint : transform.position, bounds * 2);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(currentPoint, .5f);
        
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(targetPoint, .5f);
    }

}
public enum BatRobotState
{
    Idle, Attack, Sleep
}
