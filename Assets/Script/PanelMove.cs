using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public static class PanelShow
{
    //flag = 0 -> 좌로 이동, 1 -> 우로 이동, 2 -> 상단 이동, 3 -> 하단 이동
    public static void ClosePanelUI(this RectTransform panel, float deltaTime, int moveFlag = 0)
    {
        panel.DOAnchorPosX(-panel.sizeDelta.y, deltaTime).SetEase(Ease.InOutQuad).
        onComplete += () => panel.gameObject.SetActive(false);
    }


    //flag = 0 -> 좌로 이동, 1 -> 우로 이동, 2 -> 상단 이동, 3 -> 하단 이동
    public static void ShowPanelUI(this RectTransform panel, float widthRatio, float deltaTime, int moveFlag = 0)
    {
        float moveX = Screen.width * widthRatio;
        panel.sizeDelta = new Vector2(moveX, panel.sizeDelta.y);
        if (!panel.gameObject.activeSelf)
        {
            panel.gameObject.SetActive(true);
            panel.DOAnchorPosX(-moveX, deltaTime).SetEase(Ease.InOutQuad);
        }
    }

    public static void ShowPanelUIByColor(this RectTransform panel, Color targetColor, float deltaTime)
    {
        Image panelImage = panel.GetComponent<Image>();
        if (!panel.gameObject.activeSelf)
        {
            panel.gameObject.SetActive(true);
            panelImage.DOColor(targetColor, deltaTime);
        }
    }

    public static void ShowPanelGroupUIByAlpha(this CanvasGroup group, float alpha, float duration)
    {
        group.gameObject.SetActive(true);
        group.DOFade(alpha, duration);
    }

    public static void ClosePanelGroupUIByAlpha(this CanvasGroup group, float duration)
    {
        group.DOFade(0, duration).onComplete += () => group.gameObject.SetActive(false);
    }
}
