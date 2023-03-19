using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Collectable[] content;
    [SerializeField] private float force;

    private bool activated;
    public Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activated )
        {
            activated = true;

            animator.SetTrigger("hit");

            foreach (var item in content)
            {
                var instance = Instantiate(item, transform);

                instance.transform.localPosition = Vector3.up;

                instance.Rigidbody.AddForce(new Vector2(Random.Range(-0.5f, 0.5f), 1) * force);
                instance.transform.DOScale(1, .5f)
                    .From(.2f);
            }
        }
    }
}
