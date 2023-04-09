using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 dir;

    public void Set(Vector2 dir)
    {
        this.dir = dir;
    }

    void Update()
    {
         transform.position += (Vector3)dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
