using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeInfoPanel : PanelBlocking
{
    public static NodeInfoPanel current;
    [SerializeField] private TMP_Text _rootleef;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _degree;

    void Awake(){
        current = this;
    }
    
    public void SetNodeInfo(int value, int level, int degree){
        Debug.LogWarning("SetNodeInfo");
        this._value.text = value.ToString();
        this._level.text = level.ToString();
        this._degree.text = degree.ToString();
    }

}
