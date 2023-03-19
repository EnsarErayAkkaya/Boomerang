using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPlatform : NeedLever
{
    public bool useLever;
    public Vector2 pos1;
    public Vector2 pos2;
    public float angle;
    public float speed;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = pos1;
        if (!useLever)
            StartCoroutine(LoopPlatform());
    }
    IEnumerator LoopPlatform()
    {
        Vector2 goal = pos2;
        float t = 0;
        while(true)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector2.Lerp(transform.position, goal, t);

            if ((Vector2)transform.position == pos1)
            {
                goal = pos2;
                t = 0;
            }
            else if ((Vector2)transform.position == pos2)
            {
                goal = pos1;
                t = 0;
            }
            yield return null;
        }
    }
    public override void OnLeverCall()
    {
        base.OnLeverCall();
        StartCoroutine(UsePlatformPlatformOnce());
    }
    IEnumerator UsePlatformPlatformOnce()
    {
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
        while (Vector2.Distance((Vector2)transform.position, goal) != 0f)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector2.MoveTowards(transform.position, goal, t);
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos1, 0.25f);
        Gizmos.DrawWireSphere(pos2, 0.25f);
    }
}
