using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GrabCountUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform grabCountTransform;
    [SerializeField] private GameObject grabCountPrefab;
    [SerializeField] private TextMeshProUGUI extraGrabCountText;
    [SerializeField] private int maxVisibleSpriteCount;
    private List<GameObject> grabCountObjects = new List<GameObject>();

    public void Show()
    {
        canvasGroup.DOFade(1, 0.5f).From(0);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0, 0.5f).From(1);
    }

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
            extraGrabCountText.transform.SetParent(grabCountTransform);
            extraGrabCountText.text = "+" + (val - 4).ToString();
        }
        else
        {
            extraGrabCountText.transform.SetParent(grabCountTransform.parent);
            extraGrabCountText.text = "";
        }
    }
}
