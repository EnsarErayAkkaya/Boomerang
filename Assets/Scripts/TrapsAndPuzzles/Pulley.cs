using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulley : NeedLever
{
    private Rigidbody2D rb;
    public float speed;
    public bool startTurn;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(startTurn)
            rb.MoveRotation(rb.rotation + speed * Time.fixedDeltaTime);
    }
    public override void OnLeverCall()
    {
        base.OnLeverCall();
        startTurn = true;
    }
}
