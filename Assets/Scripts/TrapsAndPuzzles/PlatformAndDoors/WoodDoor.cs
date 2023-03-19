using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodDoor : NeedLever
{
    public float startingAngle;
    public float endAngle;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, startingAngle);
    }

    public override void OnLeverCall()
    {
        base.OnLeverCall();
        StartCoroutine(Turn());
    }
    IEnumerator Turn()
    {
        float t = 0;
        Quaternion end;
        if (transform.rotation.eulerAngles.z == (360 + startingAngle) % 360)
            end = Quaternion.Euler(0, 0, endAngle);
        else
            end = Quaternion.Euler(0, 0, startingAngle);
        
        //Debug.Log("name: " +gameObject.name + ", end angle:"+ end.eulerAngles);

        while (t < 1)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, end, t);
            yield return null;
        }
    }
}
