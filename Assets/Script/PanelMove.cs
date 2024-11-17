using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public static class PanelShow
{
    public static void MovePanelUIByHorizontal(this RectTransform panel, float deltaTime, float targetX, bool activeControl = false)
    {
        bool flag = false;
        if (activeControl && panel.gameObject.activeSelf == false) {
            panel.gameObject.SetActive(true);
            flag = true;
        }
        panel.DOAnchorPosX(targetX, deltaTime).SetEase(Ease.InOutQuad).
        onComplete += () => {
            if(activeControl && panel.gameObject.activeSelf && !flag) 
                panel.gameObject.SetActive(false);
        };
    }

    public static void MovePanelUIByVertical(this RectTransform panel, float deltaTime, float targetY, bool activeControl = false){
        bool flag = false;
        
        if(activeControl && panel.gameObject.activeSelf == false) {
            panel.gameObject.SetActive(true);
            flag = true;
        }
        panel.DOAnchorPosY(targetY, deltaTime).SetEase(Ease.InOutQuad).
        onComplete += () => {
            if(activeControl && panel.gameObject.activeSelf && !flag) 
                panel.gameObject.SetActive(false);
        };
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
