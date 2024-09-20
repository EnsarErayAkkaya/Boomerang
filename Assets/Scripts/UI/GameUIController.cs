using DG.Tweening;
using System;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup playerDeadCanvasGroup;

    private LevelManager levelManager;
    
    
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelManager.OnPlayerDead.AddListener(OnPlayerDead);
    }

    private void OnDisable()
    {
        if (levelManager != null)
            levelManager.OnPlayerDead.RemoveListener(OnPlayerDead);
    }

    private void OnPlayerDead()
    {
        playerDeadCanvasGroup.gameObject.SetActive(true);
        playerDeadCanvasGroup.DOFade(1, 0.5f).From(0);
    }

    public void NewItemCollected(string name, string desc)
    {

    }
}
