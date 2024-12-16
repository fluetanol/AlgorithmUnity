using System;
using System.Collections.Generic;
using TMPro;
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


public class SortingUIManager : BaseSingleTon<SortingUIManager>
{
    public         TMP_Text TimeText;
    private static TMP_Text _timeText;
    
    public         TMP_Text ModeText;
    private static TMP_Text _modeText;

    public         TMP_InputField CountInputField;

    private         List<string> _sortUIString;
    private         ISortInfo _sortInfo;
    private         ISortControl _sortControl;
    private         ESortFlag    _flag = 0;  


    void Awake() {
        _sortUIString = new List<string> { "", "Selection", "Insertion", "Bubble", "Merge", "Quick" };
        AlgorithmSortingManager sortingManager = FindObjectOfType<AlgorithmSortingManager>();
        _sortInfo = sortingManager;
        _sortControl = sortingManager;

        _timeText = TimeText;
        _modeText = ModeText;
    }


    void Update(){
        if(Input.GetKeyDown(KeyCode.Return)) SetSort((int)_flag);
        if(!_sortInfo.IsSortFinish()){
            SetTimeText(_sortInfo.getTime().ToString("F2"));
        }
    }       

    public static void SetTimeText(string text) => _timeText.text = text;
    public static void SetModeText(string text) => _modeText.text = text;

    public void StartSort(){
        StopSort();
        _sortControl.StartSort();
    }
    public void StopSort() {
        _sortControl.StopSort();
    } 

    
    public void SetSort(int flag){
        this._flag = (ESortFlag)flag;

        if(this._flag == ESortFlag.None) return;

        if(!AlgorithmSortingManager.Instance.gameObject.activeSelf) 
            AlgorithmSortingManager.Instance.gameObject.SetActive(true);

        if(Int32.TryParse(CountInputField.text, out int result)) _sortControl.SelectSort(this._flag, result);
        else _sortControl.SelectSort(this._flag, 10);
        
        SetModeText(_sortUIString[(int)this._flag]);
    }
    

}
