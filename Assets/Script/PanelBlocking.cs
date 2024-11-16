using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelBlocking : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
