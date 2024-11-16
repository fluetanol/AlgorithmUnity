using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeUI : UIBehaviour
{
    [Header("Text")]
    public TMP_Text NodeValueText;

    [Header("Coponent")]
    public Button button;
    public Image image;

    public virtual void SetNodeValue(int value){
        NodeValueText.text = value.ToString();
    }
}

