using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class PointerEnterTrigger : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler  
{
    public UnityEvent OnPointerEnterEvent;
    public UnityEvent OnPointerExitEvent; 

    public float EaseTime = 0.1f;  

    public void OnPointerExit(PointerEventData eventData)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.DOScale(new Vector3(1f, 1f, 1f), EaseTime).SetEase(Ease.OutBack);
        OnPointerExitEvent.Invoke();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), EaseTime).SetEase(Ease.OutBack);
        OnPointerEnterEvent.Invoke();
    }
}
