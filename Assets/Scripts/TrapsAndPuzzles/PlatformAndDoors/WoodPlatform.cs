using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPlatform : NeedLever
{
    public bool useLever;
    public Vector2 pos1;
    public Vector2 pos2;
    public float angle;
    public float moveDuration = 1;
    public float waitDuration = 1;
    public Ease ease = Ease.InOutSine;

    private string id;

    private void Start()
    {
        id = Guid.NewGuid().ToString();
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = pos1;
        if (!useLever)
            LoopPlatform();
    }
    void LoopPlatform()
    {
        Vector2 goal = pos2;
        Vector2 basePoint = pos1;
        float t = 0;
        if (Vector2.Distance((Vector2)transform.position, pos1) <= 0.1f)
        {
            goal = pos2;
            basePoint = pos1;
        }
        else if (Vector2.Distance((Vector2)transform.position, pos2) <= 0.1f)
        {
            goal = pos1;
            basePoint = pos2;
        }

        var sequence = DOTween.Sequence();

        sequence.AppendInterval(waitDuration);
        sequence.Append(transform.DOMove(goal, moveDuration).SetEase(ease));
        sequence.AppendCallback(() => LoopPlatform());
    }

    public override void OnLeverCall()
    {
        base.OnLeverCall();
        UsePlatformPlatformOnce();
    }

    void UsePlatformPlatformOnce()
    {
        DOTween.Kill(id + "platform_lever_call");

        Vector2 goal = pos2;
        float t = 0;
        if (Vector2.Distance((Vector2)transform.position, pos1) <= 0.1f)
        {
            goal = pos2;
        }
        else if (Vector2.Distance((Vector2)transform.position, pos2) <= 0.1f)
        {
            goal = pos1;
        }

        transform.DOMove(goal, moveDuration).SetEase(Ease.InOutSine).SetId(id + "platform_lever_call");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos1, 0.25f);
        Gizmos.DrawWireSphere(pos2, 0.25f);
    }
}
