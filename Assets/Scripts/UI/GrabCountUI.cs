using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrabCountUI : MonoBehaviour
{
    [SerializeField] private Transform grabCountTransform;
    [SerializeField] private GameObject grabCountPrefab;
    [SerializeField] private TextMeshProUGUI extraGrabCountText;
    [SerializeField] private int maxVisibleSpriteCount;
    private List<GameObject> grabCountObjects = new List<GameObject>();

    public void SetGrabCount(int val)
    {
        if (val > grabCountObjects.Count)
        {
            while (grabCountObjects.Count < Mathf.Min(4, val))
            {
                grabCountObjects.Add(Instantiate(grabCountPrefab, grabCountTransform));
            }
        }
        else
        {
            for (int i = grabCountObjects.Count; i > val; i--)
            {
                Destroy(grabCountObjects[i - 1]);
                grabCountObjects.RemoveAt(i - 1);
            }
        }

        if (val > maxVisibleSpriteCount)
        {
            extraGrabCountText.transform.parent = grabCountTransform;
            extraGrabCountText.text = "+" + (val - 4).ToString();
        }
        else
        {
            extraGrabCountText.transform.parent = grabCountTransform.parent;
            extraGrabCountText.text = "";
        }
    }
}
