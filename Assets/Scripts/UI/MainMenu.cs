using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public void startButtonOnClick()
    {
        SceneManager.LoadScene("BaseScene");
    }
}
