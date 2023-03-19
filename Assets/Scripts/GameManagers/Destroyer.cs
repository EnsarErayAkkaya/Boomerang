using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public bool isTrigger;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isTrigger)
            Destroy(collision.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger)
            Destroy(collision.gameObject);
    }
}
