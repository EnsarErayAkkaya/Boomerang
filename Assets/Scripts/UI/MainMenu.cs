using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject newGameButton;

    [SerializeField] Sprite continueSprite;
    [SerializeField] Sprite startSprite;

    private void Start() 
    {
        SaveService.LoadGame();
        if (SaveService.saveData.currentLevel != 0)
        {
            newGameButton.SetActive(true);

            startButton.GetComponent<Image>().sprite = continueSprite;
        }
        else
        {
            newGameButton.SetActive(false);

            startButton.GetComponent<Image>().sprite = startSprite;
        }
    }

    private void Update() 
    {
        
    }

    public void StartButtonOnClick()
    {
        Debug.Log("current level: " + SaveService.saveData.currentLevel);
        SceneManager.LoadScene(SaveService.saveData.currentLevel);
    }

    public void NewGameButtonOnClick()
    {
        SaveService.saveData.currentLevel = 0;

        SceneManager.LoadScene(SaveService.saveData.currentLevel);

        SaveService.SaveGame();
    }

}


