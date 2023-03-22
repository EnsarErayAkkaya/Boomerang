using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool isClimbing;
    private float x;
    private Vector2 moveVector;
    private bool isGrounded;
    private bool isCrouching;

    public BoxCollider2D BoxCollider => boxCollider;
    void Update()
    {
        if (!isGrounded && !isClimbing)
            moveVector.y -= gravity * Time.deltaTime;
        else if (isGrounded && moveVector.y < 0)
        {
            moveVector.y = 0;
        }

        if(Input.GetKeyDown(KeyCode.S) && !boomerangController.hasBumerang)
        {
            isCrouching = true;
            boxCollider.size = boxColliderCrouchingSize;
            boxCollider.offset = boxColliderCrouchingOffset;
        }
        else if((Input.GetKeyUp(KeyCode.S) && !boomerangController.hasBumerang) || isCrouching && boomerangController.hasBumerang)
        {
            isCrouching = false;
            boxCollider.size = boxColliderNormalSize;
            boxCollider.offset = boxColliderNormalOffset;
        }

        if (!boomerangController.hasBumerang && !isCrouching)
        {
            x = Input.GetAxis("Horizontal");

            if (isClimbing && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
            {
                moveVector.y = ladderClimbingSpeed;
            }
            else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
            {
                moveVector.y += jumpForce;
            }

            moveVector.x = x * speed;
        }

        animator.SetBool("isCrouching", isCrouching);
        animator.SetFloat("Speed", x);
        animator.SetFloat("JumpSpeed", moveVector.y);
    }
    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        if (!boomerangController.hasBumerang && !isCrouching)
            rb.velocity = moveVector;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("BadRobot"))
        {
            gameObject.SetActive(false);
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
        if (hit.collider != null)
        {
            transform.SetParent(hit.transform);
        }
        else
        {
            transform.SetParent(null);
        }
        return hit.collider != null;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
