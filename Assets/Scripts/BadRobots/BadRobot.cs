using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BadRobot : MonoBehaviour, IEnemy
{
    public BadRobotState badRobotState;
    public Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    public float gravity;
    public float directionChooseInterval;
    public BoxCollider2D boxCollider;
    public LayerMask groundLayers;
    public LayerMask playerDetectLayers;
    public Animator animator;
    public Vector2 head;
    public float rayCheckDist;
    public Vector2 jumpRayPoint;
    public Vector2 canJumpRayPoint;
    public float minJumpInterval;
    public float sleepDuration;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sleepSfx;
    [SerializeField] private AudioClip wakeUpSfx;
    [SerializeField] private AudioClip jumpSfx;

    private bool isActivated = true;
    private Transform player;
    private Vector2 moveVector;
    private bool isGrounded;
    private float lastJumpTime;
    private bool sleep;
    private bool wallInFront;
    private bool stepIsValid;

    public bool IsActivated => isActivated;

    private async void Start()
    {
        await Task.Delay(100);
        player = FindObjectOfType<CharacterController>().transform;
        StartCoroutine(ChooseDirection());
    }

    private void FixedUpdate()
    {
        if (!isActivated) return;

        isGrounded = IsGrounded();

        if (!isGrounded)
            moveVector.y -= gravity * Time.deltaTime;
        else if (isGrounded && moveVector.y < 0)
        {
            moveVector.y = 0;
        }

        if (!sleep && isActivated)
        {
            stepIsValid = IsNextStepValid();
            if ( stepIsValid )
            {
                CheckAndSetJump();
                LookForPlayer();

                animator.SetFloat("Speed", moveVector.x);
                animator.SetFloat("JumpSpeed", moveVector.y);
            }
        }

        rb.velocity = moveVector;
    }
    private void LookForPlayer()
    {
        if (player != null)
        {
            Vector2 rayDir = (Vector2)player.position - (rb.position + head);
            float dirMagnitude = rayDir.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(rb.position + head, rayDir.normalized, dirMagnitude, playerDetectLayers);

            Debug.DrawRay(rb.position + head, rayDir * dirMagnitude, Color.blue);
            if (hit.collider != null)
            {
                //Debug.Log(gameObject.name + " found " + hit.collider.gameObject.name);

                if (hit.collider.CompareTag("Player"))
                {
                    badRobotState = BadRobotState.Found;
                    moveVector.x = hit.transform.position.x > rb.position.x ? 1 : -1;
                }
                else
                {
                    badRobotState = BadRobotState.Patrolling;
                }
            }
        }
        else
        {
            player = FindObjectOfType<CharacterController>().transform;
        }
    }
    IEnumerator ChooseDirection()
    {
        while (!sleep)
        {   
            int dir = Random.Range(-1, 2);
            moveVector.x = (dir > 0 ? 1 : dir < 0 ? -1 : 0) * speed;
            moveVector.Normalize();
            yield return new WaitForSeconds(directionChooseInterval);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boomerang"))
        {
            sleep = true;
            boxCollider.isTrigger = true;
            moveVector.x = 0;

            PlaySound(sleepSfx);

            //rb.bodyType = RigidbodyType2D.Static;
            animator.SetTrigger("Sleep");
            StartCoroutine(WaitingToWakeUp());
        }
    }
    private IEnumerator WaitingToWakeUp()
    {
        badRobotState = BadRobotState.Sleeping;
        yield return new WaitForSeconds(1);
        float t = 0;
        while (t < sleepDuration)
        {
            t += Time.deltaTime;
            animator.SetFloat("WaitWakeMultiplier", ( t / (sleepDuration / 4f) ));

            yield return null;
        }
        animator.speed = 1;
        boxCollider.isTrigger = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        animator.SetTrigger("Wake");

        PlaySound(wakeUpSfx);
        sleep = false;
    }
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .05f, groundLayers);
        if (hit.collider != null)
        {
            if (sleep)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }

            if (hit.collider && hit.collider.CompareTag("WoodPlatform") && (transform.parent == null))
            {
                transform.SetParent(hit.transform);
            }
        }
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        return hit.collider != null;
    }
    public bool IsNextStepValid()
    {
        Vector2 v = moveVector / 2;
        v.y = 0;
        Vector2 dir = v - head;
        //Debug.Log(dir);
        dir.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(rb.position + head, dir, rayCheckDist, groundLayers);
        Debug.DrawRay(rb.position + head,dir * rayCheckDist, Color.blue);
        return hit.collider != null;
    }
    public void CheckAndSetJump()
    {
        if (moveVector.x == 0 || !isGrounded || moveVector.y > 0 || lastJumpTime + minJumpInterval > Time.time) return;

        Vector2 dir = moveVector;
        dir.y = 0;
        dir.Normalize();

        RaycastHit2D hit1 = Physics2D.Raycast(rb.position + canJumpRayPoint, dir, .5f, groundLayers);
        Debug.DrawRay(rb.position + canJumpRayPoint, dir * .5f, Color.red);
        wallInFront = true;
        if (hit1.collider != null)
        {
            wallInFront = true;
            return;
        }
        wallInFront = false;

        RaycastHit2D hit = Physics2D.Raycast(rb.position + jumpRayPoint, dir, .5f, groundLayers);
        Debug.DrawRay(rb.position + jumpRayPoint, dir * .5f, Color.magenta);
        if(hit.collider != null && isGrounded)
        {
            lastJumpTime = Time.time;
            //Debug.Log("bot jump, hit: " + hit.collider.gameObject.name);
            moveVector.y += jumpForce;
            PlaySound(jumpSfx);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + head, 0.2f);
        Gizmos.DrawWireSphere((Vector2)transform.position + jumpRayPoint, 0.2f);
        Gizmos.DrawWireSphere((Vector2)transform.position + canJumpRayPoint, 0.2f);
    }

    public void ToggleActivation(bool isActivated)
    {
        this.isActivated = isActivated;

        if (!isActivated)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
public enum BadRobotState
{
    Patrolling, Found, Sleeping
}