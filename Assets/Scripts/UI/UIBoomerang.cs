using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoomerang : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float revSpeed;

    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = Random.insideUnitCircle.normalized;

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

        rb.MoveRotation(rb.rotation + revSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "YWall")
        {
            direction.y *= -1;
        }

        if (collision.gameObject.tag == "XWall")
        {
            direction.x *= -1;
        }
    }
}
