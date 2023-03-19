using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;

    public Rigidbody2D Rigidbody => rigidbody;
    public virtual void OnCollect()
    {

    }
}
