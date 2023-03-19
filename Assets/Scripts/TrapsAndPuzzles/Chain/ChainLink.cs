using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLink : MonoBehaviour
{
    public int index;
    public HingeJoint2D hJoint;
    private Chain chain;
    private bool unlinked;
    private bool collidersSetted;
    private void Start()
    {
        chain = transform.parent.GetComponent<Chain>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        chain.OnChainGethit(index);
    }
    public void Unlink()
    {
        if (!unlinked)
        {
            unlinked = true;
            hJoint.connectedBody = GetComponent<Rigidbody2D>();
        }
        StartCoroutine(SetColliders());
    }
    public void SetCollidersWithoutUnlink()
    {
        StartCoroutine(SetColliders());
    }
    IEnumerator SetColliders()
    {
        if (!collidersSetted)
        {
            collidersSetted = true;
            yield return new WaitForSeconds(0.25f);

            GetComponent<CapsuleCollider2D>().enabled = false;
            if (transform.childCount > 0)
                transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
