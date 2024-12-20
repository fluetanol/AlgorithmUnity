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
    [SerializeField] private float    _colorFadeTime;
    [SerializeField] private float    _panelFadeTime;
    [SerializeField] private TMP_Text _titleText;

    RectTransform rectTransform;
    void Awake() {
        DontDestroyOnLoad(transform.parent.gameObject);
        rectTransform = GetComponent<RectTransform>();
    }

    public void TitleSet(String title){
        _titleText.text = title;
    }

    public void TitleOpen(){
        Sequence s = DOTween.Sequence();
        DOTween.To(() => rectTransform.offsetMin,
        x => rectTransform.offsetMin = x,
        new Vector2(0, rectTransform.offsetMin.y), 0.5f);

        s.Append(_titleText.DOFade(1, _colorFadeTime));

    }

    public void TitleClose(){
        Sequence s = DOTween.Sequence();
        Color color = GetComponent<Image>().color;
        color.a = 0;
        s.Append(rectTransform.DOHidePanelUIByColor(color, _panelFadeTime));
    }


}
