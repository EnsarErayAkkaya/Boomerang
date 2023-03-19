using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyBandChain : MonoBehaviour
{
    private HingeJoint2D hJoint;
    private Rigidbody2D connectedChain;
    private void Start()
    {
        hJoint = GetComponent<HingeJoint2D>();
        connectedChain =  hJoint.connectedBody;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Pulley p = collision.transform.GetComponent<Pulley>();
        if ( p != null)
        {
            hJoint.connectedBody = p.GetComponent<Rigidbody2D>();
        }
        else
        {
            hJoint.connectedBody = connectedChain;
        }
    }
}
