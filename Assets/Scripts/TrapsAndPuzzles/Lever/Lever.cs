using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool canActivateMoreThenOnce;
    private bool activated;
    private int activationCount = 0;
    public Animator animator;
    public List<NeedLever> needLever;
    public AudioSource leverSfx;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activated || canActivateMoreThenOnce)
        {
            needLever.ForEach(s => s.OnLeverCall());
            activated = true;
            activationCount++;

            leverSfx.Play();

            if(activationCount % 2 == 1)
                animator.SetTrigger("hit");
            else
                animator.SetTrigger("hitReverse");
        }   
    }
}
