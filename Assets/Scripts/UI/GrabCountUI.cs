using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrabCountUI : MonoBehaviour
{
    public TextMeshProUGUI grabCountText;

    public void SetGrabCount(int c)
    {
        grabCountText.text = c.ToString();
    }
}
