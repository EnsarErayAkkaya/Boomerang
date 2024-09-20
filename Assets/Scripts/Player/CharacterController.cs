using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private BoomerangController boomerangController;
    [SerializeField] private float speed;
    [SerializeField] private float ladderClimbingSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Animator animator;

    [SerializeField] private Vector2 boxColliderCrouchingSize;
    [SerializeField] private Vector2 boxColliderNormalSize; 
    [SerializeField] private Vector2 boxColliderCrouchingOffset;
    [SerializeField] private Vector2 boxColliderNormalOffset;

    [Header("Death")]
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private float ragdollSpawnForce;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip walkSfx;
    [SerializeField] private float walkSfxInterval;

    private bool characterDisabled = false;
    private bool isClimbing;
    private float x;
    private Vector2 moveVector;
    private bool isGrounded;
    private bool isCrouching;
    private float lastWalkSfxTime;

    public BoxCollider2D BoxCollider => boxCollider;
    public BoomerangController BoomerangController => boomerangController;
    void Update()
    {
        if (!characterDisabled)
        {
            if (!isGrounded && !isClimbing)
                moveVector.y -= gravity * Time.deltaTime;
            else if (isGrounded && moveVector.y < 0)
            {
                moveVector.y = 0;
            }

            if (isGrounded && !isCrouching && moveVector.x != 0)
            {
                if (lastWalkSfxTime + walkSfxInterval < Time.time)
                {
                    lastWalkSfxTime = Time.time;
                    PlaySound(walkSfx, Random.Range(0.95f, 1.05f));
                }
            }
            else
            {
                StopWalkSfx();
            }

            if (Input.GetKeyDown(KeyCode.S) && !boomerangController.HasBumerang)
            {
                isCrouching = true;
                boxCollider.size = boxColliderCrouchingSize;
                boxCollider.offset = boxColliderCrouchingOffset;
            }
            else if ((Input.GetKeyUp(KeyCode.S) && !boomerangController.HasBumerang) || isCrouching && boomerangController.HasBumerang)
            {
                isCrouching = false;
                boxCollider.size = boxColliderNormalSize;
                boxCollider.offset = boxColliderNormalOffset;
            }

            if (!boomerangController.HasBumerang && !isCrouching)
            {
                x = Input.GetAxis("Horizontal");

                if (isClimbing)
                {
                    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                    {
                        moveVector.y = ladderClimbingSpeed;
                    }
                    else
                    {
                        moveVector.y = 0;
                    }
                }
                else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
                {
                    PlaySound(jumpSfx);
                    moveVector.y += jumpForce;
                }

                moveVector.x = x * speed;
            }

            animator.SetBool("isCrouching", isCrouching);
            animator.SetFloat("Speed", x);
            animator.SetFloat("JumpSpeed", moveVector.y);
        }
    }
    private void FixedUpdate()
    {
        if (!characterDisabled)
        {
            isGrounded = IsGrounded();
            if (!boomerangController.HasBumerang && !isCrouching)
                rb.velocity = moveVector;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("BadRobot"))
        {
            Die("Bad Robot");
            //gameObject.SetActive(false);
        }
        else if (collision.transform.TryGetComponent<Collectable>(out var collectable))
        {
            collectable.OnCollect();
            Destroy(collectable.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Ladder>() != null)
        {
            isClimbing = true;
        }
        else if (collider.TryGetComponent<Projectile>(out var projectile))
        {
            Die("projectile");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Ladder>() != null)
        {
            isClimbing = false;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .05f, groundLayers);

        if (hit.collider && hit.collider.CompareTag("WoodPlatform"))
        {
            transform.SetParent(hit.transform);
        }
        else if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        return hit.collider != null;
    }

    public void Die(string reason)
    {
        Debug.Log("Die reason: " + reason);

        FindObjectOfType<SpawnPoint>().PlaySound(deathSfx);

        FindObjectOfType<LevelManager>().SetPlayerDead();

        gameObject.SetActive(false);

        var instance = Instantiate(ragdoll, transform.position, Quaternion.identity);

        int childCount = instance.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = instance.transform.GetChild(i);

            float force = Random.Range(0.9f, 1.1f) * ragdollSpawnForce;
            var forceDir = ((Vector2)child.localPosition).normalized;

            child.GetComponent<Rigidbody2D>().AddForce(force * forceDir, ForceMode2D.Impulse);
            child.GetComponent<Rigidbody2D>().AddTorque(force * .1f, ForceMode2D.Impulse);
        }

    }

    public void DisableCharacter()
    {
        characterDisabled = true;

        animator.speed = 0;
    }

    public void ActivateCharacter()
    {
        characterDisabled = false;

        animator.speed = 1;
    }

    public void PlaySound(AudioClip clip, float pitch = 1)
    {
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.Play();
    }

    public void StopWalkSfx()
    {
        if (audioSource.clip == walkSfx)
        {
            audioSource.Stop();
        }
    }

    public void OnGrabBoomerang()
    {
        StopWalkSfx();

        moveVector = Vector3.zero;
        animator.SetFloat("Speed", 0);
        animator.SetFloat("JumpSpeed", 0);
        animator.SetBool("isCrouching", false);
    }
}
