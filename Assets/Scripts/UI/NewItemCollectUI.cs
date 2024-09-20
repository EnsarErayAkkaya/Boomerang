using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewItemCollectUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI relicName;
    [SerializeField] private TextMeshProUGUI relicDesc;
    [SerializeField] private RectTransform okayButton;
    [SerializeField] private AudioSource audioSource;

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Show(string _relicame, string _relicDesc)
    {
        levelManager.StopAllGame();
        canvasGroup.gameObject.SetActive(true);
        relicName.text = _relicame;
        relicDesc.text = _relicDesc;

        audioSource.Play();

        canvasGroup.DOFade(1, 0.35f).From(0);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0, 0.5f).From(1)
            .OnComplete(() =>
            {
                canvasGroup.gameObject.SetActive(false);
                levelManager.StartAllGame();
            }); 
    }
}
