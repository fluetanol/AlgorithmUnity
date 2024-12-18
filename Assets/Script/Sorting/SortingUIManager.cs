using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

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
    public         TMP_Text       TimeText;
    public         TMP_Text       ModeText;
    public         TMP_InputField CountInputField;
    public         Button         ConfirmButton;
    public         Button         StartButton;

    private         List<string> _sortUIString;
    private         ISortInfo _sortInfo;
    private         ISortControl _sortControl;
    private         ESortFlag    _flag = 0;  


    void Awake() {
        _sortUIString = new List<string> { "", "Selection", "Insertion", "Bubble", "Merge", "Quick" };
        AlgorithmSortingManager sortingManager = FindObjectOfType<AlgorithmSortingManager>();
        _sortInfo = sortingManager;
        _sortControl = sortingManager;
        ConfirmButton.onClick.AddListener(() => {SetSort((int)_flag);});
    }


    void Update(){
        if(!_sortInfo.IsSortFinish()){
            SetTimeText(_sortInfo.getTime().ToString("F2"));
        }
    }       

    public void SetTimeText(string text) => TimeText.text = text;
    public void SetModeText(string text) => ModeText.text = text;

    public void ToogleMix(bool isMix){
        _sortControl.SetMix(isMix);
    }

    public void StartSort(){
        StopSort();
        _sortControl.ResetTime();
        _sortControl.StartSort();
    }
    public void StopSort() {
        _sortControl.StopSort();
        _sortControl.SortForceFinish();
    } 



    
    public void SetSort(int flag){
        this._flag = (ESortFlag)flag;

        if(this._flag == ESortFlag.None) return;

        if(Int32.TryParse(CountInputField.text, out int result) && result >= 3 && result <= 30) {
             _sortControl.SelectSort(this._flag, result);
        }
        else if(result > 30){
            _sortControl.SelectSort(this._flag, 30);
        }
        else if(result < 3) {
            _sortControl.SelectSort(this._flag, 3);
        }
        else{
            _sortControl.SelectSort(this._flag, 10);
        }

        SetModeText(_sortUIString[(int)this._flag]);
    }
    

}
