using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class NodeInfoPanelManager : MonoBehaviour, IPointerDownHandler
{
    public static NodeInfoPanelManager current;
    [SerializeField] private TMP_Text rootleef;
    [SerializeField] private TMP_Text value;
    [SerializeField] private TMP_Text level;
    [SerializeField] private TMP_Text degree;

    void Awake(){
        current = this;
    }
    
    public void SetNodeInfo(int value, int level, int degree){
        this.value.text = value.ToString();
        this.level.text = level.ToString();
        this.degree.text = degree.ToString();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData){
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
