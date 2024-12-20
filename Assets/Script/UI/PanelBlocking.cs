using UnityEngine;
using UnityEngine.EventSystems;

public class PanelBlocking : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
