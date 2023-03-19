using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public BoomerangController boomerangController;
    public float speed;
    public float jumpForce;
    public float gravity;
    public BoxCollider2D boxCollider;
    public Rigidbody2D rb;
    public LayerMask groundLayers;
    public Animator animator;

    public Vector2 boxColliderCrouchingSize;
    public Vector2 boxColliderNormalSize; 
    public Vector2 boxColliderCrouchingOffset;
    public Vector2 boxColliderNormalOffset; 


    private float x;
    private Vector2 moveVector;
    private bool isGrounded;
    private bool isCrouching;
    void Update()
    {
        if (!isGrounded)
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
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
                moveVector.y += jumpForce;

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
}
