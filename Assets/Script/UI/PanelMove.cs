using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 패널들을 도트윈 애니메이션을 이용해 보여주는 기능을 제공하는 클래스
/// </summary>
public static class PanelShow
{
    // 패널을 수평으로 이동시키는 메소드
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

    // 패널을 수직으로 이동시키는 메소드
    public static void MovePanelUIByVertical(this RectTransform panel, float deltaTime, float targetY, bool activeControl = false){
        bool flag = false;
        
        if(activeControl && panel.gameObject.activeSelf == false) {
            panel.gameObject.SetActive(true);
            flag = true;
        }
        panel.DOAnchorPosY(targetY, deltaTime).SetEase(Ease.InOutQuad).
        onComplete += () => {
            Debug.Log("??");
            if(activeControl && panel.gameObject.activeSelf && !flag) 
                panel.gameObject.SetActive(false);
        };
    }

    
    public static Tweener DOShowPanelUIByColor(this RectTransform panel, Color targetColor, float deltaTime)
    {
        Tweener tw = null;
        Image panelImage = panel.GetComponent<Image>();
        if (!panel.gameObject.activeSelf)
        {
            panel.gameObject.SetActive(true);
            tw = panelImage.DOColor(targetColor, deltaTime);
        }
        return tw;
    }

    public static Tweener DOHidePanelUIByColor(this RectTransform panel, Color targetColor, float deltaTime, Ease ease = Ease.InOutQuad)
    {
        Tweener tw = null;
        Image panelImage = panel.GetComponent<Image>();
        if (panel.gameObject.activeSelf)
        {
            tw = panelImage.DOColor(targetColor, deltaTime).SetEase(ease);
            tw.onComplete += () => panel.gameObject.SetActive(false);
        }
        return tw;
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
