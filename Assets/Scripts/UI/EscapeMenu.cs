using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private LevelManager levelManager;
    private bool enabled;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Show()
    {
        DOTween.Kill("escape");

        levelManager.StopAllGame();
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.35f).From(0).SetId("escape");
    }

    public void Hide()
    {
        DOTween.Kill("escape");

        canvasGroup.DOFade(0, 0.35f).From(1)
            .OnComplete(() =>
            {
                levelManager.StartAllGame();
                canvasGroup.gameObject.SetActive(false);
            }).SetId("escape");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (enabled)
            {
                enabled = false;
                Hide();
            }
            else
            {
                enabled = true;
                Show();
            }
        }
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume()
    {
        Hide();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
