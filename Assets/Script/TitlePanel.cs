using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface ITitlePlay
{
    void TitleSet(String title);
    void TitleOpen();
    void TitleClose();
}

public class TitlePanel : MonoBehaviour, ITitlePlay
{
    [SerializeField] private float ColorFadeTime;
    [SerializeField] private float PanelFadeTime;
    [SerializeField] private TMP_Text titleText;

    RectTransform rectTransform;
    void Awake() {
        DontDestroyOnLoad(transform.parent.gameObject);
        rectTransform = GetComponent<RectTransform>();
    }

    public void TitleSet(String title){
        titleText.text = title;
    }

    public void TitleOpen(){
        Sequence s = DOTween.Sequence();
        DOTween.To(() => rectTransform.offsetMin,
        x => rectTransform.offsetMin = x,
        new Vector2(0, rectTransform.offsetMin.y), 0.5f);

        s.Append(titleText.DOFade(1, ColorFadeTime));

    }

    public void TitleClose(){
        Sequence s = DOTween.Sequence();
        Color color = GetComponent<Image>().color;
        color.a = 0;
        s.Append(rectTransform.DOHidePanelUIByColor(color, PanelFadeTime));
    }


}
