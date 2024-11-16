using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeInfoPanelManager : PanelBlocking
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

}
