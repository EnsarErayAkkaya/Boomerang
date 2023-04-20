using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private float textRevealDuration;
    [SerializeField] private float appearDelay;

    private bool skip;
    private bool upOriantation;

    private Camera _camera;
    private Vector2 backgroundSize;
    private Vector2 pointerSize;
    private RectTransform tooltipRect;
    public RectTransform Background => background;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();

        backgroundSize = background.sizeDelta;
        pointerSize = pointer.sizeDelta;

        tooltipRect = canvasGroup.GetComponent<RectTransform>();
    }

    public void Set(Vector3 worldPos, Vector2 offset, string[] texts, Action onComplete)
    {
        Vector2 screenPos = _camera.WorldToScreenPoint(worldPos);

        var topRightScreen = _camera.ViewportToScreenPoint(new Vector3(1, 1, 0));

        float totalHeight = (backgroundSize.y * 2) + pointerSize.y + offset.y;

        float height = (screenPos.y + totalHeight);

        float maxHeight = topRightScreen.y;

        if (height > maxHeight)
        {
            upOriantation = false;
            screenPos -= offset;
        }
        else
        {
            upOriantation = true;
            screenPos += offset;
        }

        float deltaX = 0;

        if (screenPos.x - (backgroundSize.x) < 0)
        {
            deltaX = (backgroundSize.x) - screenPos.x;
        }
        else if (screenPos.x + (backgroundSize.x) > topRightScreen.x)
        {
            deltaX = topRightScreen.x - (screenPos.x + backgroundSize.x);
        }

        Vector2 finalPos = screenPos;

        finalPos.y += upOriantation ? (backgroundSize.y + pointer.sizeDelta.y) : (-backgroundSize.y - pointerSize.y);

        finalPos.x += deltaX;

        if (upOriantation)
        {
            pointer.anchoredPosition = new Vector2(-deltaX * .5f, -backgroundSize.y * .5f);
            pointer.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            pointer.anchoredPosition = new Vector2(-deltaX * .5f, backgroundSize.y * .5f);
            pointer.rotation = Quaternion.Euler(0, 0, 180);
        }

        tooltipRect.position = finalPos;

        StartCoroutine(EnumerateText(texts, onComplete));
    }

    private IEnumerator EnumerateText(string[] texts, Action onComplete)
    {
        yield return new WaitForSeconds(appearDelay);

        canvasGroup.gameObject.SetActive(true);

        canvasGroup.DOFade(1, appearDelay)
            .From(0);
        
        int currentTextIndex = 0;

        while (currentTextIndex < texts.Length)
        {
            int nextLetterIndex = 0;
            int textLength = texts[currentTextIndex].Length;

            while (nextLetterIndex < textLength)
            {
                textUI.text += texts[currentTextIndex][nextLetterIndex++];

                if (skip == true)
                {
                    break;
                }

                yield return new WaitForSeconds(textRevealDuration);
            }

            currentTextIndex++;

            yield return new WaitUntil(() => skip);

            textUI.text = "";
            skip = false;
        }

        canvasGroup.DOFade(0, appearDelay)
            .OnComplete(() =>
            {
                canvasGroup.gameObject.SetActive(false);
                onComplete();
            });
    }

    public void SkipButton()
    {
        skip = true;
    }
}
