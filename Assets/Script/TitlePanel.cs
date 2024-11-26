using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    [SerializeField] private float ColorFadeTime;
    [SerializeField] private float PanelFadeTime;

    // Start is called before the first frame update
    void Start()
    {
        Sequence s = DOTween.Sequence();
        RectTransform rectTransform = GetComponent<RectTransform>();
        
        s.Append(DOTween.To(() => rectTransform.offsetMin, 
        x => rectTransform.offsetMin = x, 
        new Vector2(0, rectTransform.offsetMin.y), 1f));

        s.Append(GetComponentInChildren<TMP_Text>().DOFade(1, ColorFadeTime));
        s.AppendInterval(1f);
        Color color = GetComponent<Image>().color;
        color.a = 0;
        s.Append(rectTransform.DOHidePanelUIByColor(color, PanelFadeTime));
    }



}
