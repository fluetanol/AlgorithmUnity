using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum ESortFlag
{
    None,
    Selection,
    Insertion,
    Bubble,
    Merge,
    Quick
}

public class SortingUIManager : MonoBehaviour
{
    public TMP_Text TimeText;
    public TMP_Text ModeText;
    public TMP_InputField CountInputField;
    public static SortingUIManager Instance;

    private List<string> _sortUIString;
    private ESortFlag flag = 0;
    private ISortSelect _sortSelect;


    void Awake() {
        Instance = this;
        _sortUIString = new List<string> { "", "Selection", "Insertion", "Bubble", "Merge", "Quick" };
        _sortSelect = AlgorithmSortingManager.Instance;
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Return)) SetSort((int)flag);
    }

    public void SetTimeText(string text) => TimeText.text = text;
    public void SetModeText(string text) => ModeText.text = text;
    public void StartSort() => SetSort((int)flag);

    public void SetSort(int flag)
    {
        this.flag = (ESortFlag)flag;

        if(this.flag == ESortFlag.None) return;

        if(!AlgorithmSortingManager.Instance.gameObject.activeSelf) 
            AlgorithmSortingManager.Instance.gameObject.SetActive(true);

        if(Int32.TryParse(CountInputField.text, out int result)) _sortSelect.SelectSort(this.flag, result);
        else _sortSelect.SelectSort(this.flag, 10);
        
        SetModeText(_sortUIString[(int)this.flag]);
    }
    

}
