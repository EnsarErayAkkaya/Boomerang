using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject startButtonText;
    [SerializeField] GameObject startNewGameButton;

    private void Start() 
    {
        SaveService.LoadGame();
        if (SaveService.saveData.currentLevel != 0)
        {
            startButtonText.GetComponent<TextMeshProUGUI>().text = "CONTINUE";

            startNewGameButton.SetActive(true);
        }
        else
        {
            startButtonText.GetComponent<TextMeshProUGUI>().text = "START";

            startNewGameButton.SetActive(false);
        }
    }

    public void StartButtonOnClick()
    {
        Debug.Log("current lebvel: " + SaveService.saveData.currentLevel);
        SceneManager.LoadScene(SaveService.saveData.currentLevel);
    }

    public void StartNewGameButtonOnClick()
    {
        SaveService.saveData.currentLevel = 0;

        SceneManager.LoadScene(SaveService.saveData.currentLevel);

        SaveService.SaveGame();
    }

}


