using System;
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


    private ESortFlag flag = 0;
    private SortInterface _sortInterface;

    void Awake() {
        Instance = this;
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Return)) SetSort((int)flag);
    }

    public void SetTimeText(string text) => TimeText.text = text;
    public void SetModeText(string text) => ModeText.text = text;

    public void SetSort(int flag)
    {
        this.flag = (ESortFlag)flag;

        if(this.flag == ESortFlag.None) return;

        if(!AlgorithmSortingManager.Instance.gameObject.activeSelf) 
            AlgorithmSortingManager.Instance.gameObject.SetActive(true);
        if(Int32.TryParse(CountInputField.text, out int result)) 
            AlgorithmSortingManager.Instance.InitializeSetting(result);
        else AlgorithmSortingManager.Instance.InitializeSetting(10);

        switch(this.flag){
            case ESortFlag.Selection:
            ModeText.text = "Selection";
            AlgorithmSortingManager._sortInterface = new SelectionSort(AlgorithmSortingManager.Instance._sortList, AlgorithmSortingManager.Instance._sortObject);
            break;
            
            case ESortFlag.Insertion:
            ModeText.text = "Insertion";
            AlgorithmSortingManager._sortInterface = new InsertionSort(AlgorithmSortingManager.Instance._sortList, AlgorithmSortingManager.Instance._sortObject);
            break;

            case ESortFlag.Bubble:
            ModeText.text = "Bubble";
            AlgorithmSortingManager._sortInterface = new BubbleSort(AlgorithmSortingManager.Instance._sortList, AlgorithmSortingManager.Instance._sortObject);
            break;

            case ESortFlag.Merge:
            ModeText.text = "Merge";
            AlgorithmSortingManager._sortInterface = new MergeSort(AlgorithmSortingManager.Instance._sortList, AlgorithmSortingManager.Instance._sortObject);
            break;
            
            default:
            break;
        }

    }

}
