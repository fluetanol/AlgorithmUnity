using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum SortFlag
{
    Insertion,
    Selection
}

public class SortingUIManager : MonoBehaviour
{
    public TMP_Text TimeText;
    public TMP_Text ModeText;
    public TMP_InputField CountInputField;
    public static SortingUIManager Instance;

    private int flag = 0;

    void Awake() => Instance = this;
    void Update(){
        if(Input.GetKeyDown(KeyCode.Return)) SetSort(flag);
    }

    public void SetTimeText(string text) => TimeText.text = text;
    public void SetModeText(string text) => ModeText.text = text;
    public void SetSort(int flag)
    {
        this.flag = flag;
        if(!AlgorithmSortingManager.Instance.gameObject.activeSelf) AlgorithmSortingManager.Instance.gameObject.SetActive(true);
        if(Int32.TryParse(CountInputField.text, out int result)) AlgorithmSortingManager.Instance.InitializeSetting(result);
        else AlgorithmSortingManager.Instance.InitializeSetting(10);

        switch(flag){
            case 0:
            ModeText.text = "Sort: Selection";
            AlgorithmSortingManager._sortInterface = new SelectionSort(AlgorithmSortingManager.Instance._sortList, AlgorithmSortingManager.Instance._sortObject);
            break;
            
            case 1:
            ModeText.text = "Sort: Insertion";
            AlgorithmSortingManager._sortInterface = new InsertionSort(AlgorithmSortingManager.Instance._sortList, AlgorithmSortingManager.Instance._sortObject);
            break;

            case 2:
            ModeText.text = "Sort: Bubble";
            AlgorithmSortingManager._sortInterface = new BubbleSort(AlgorithmSortingManager.Instance._sortList, AlgorithmSortingManager.Instance._sortObject);
            break;

            case 3:
            ModeText.text = "Sort: Merge";
            AlgorithmSortingManager._sortInterface = new MergeSort(AlgorithmSortingManager.Instance._sortList, AlgorithmSortingManager.Instance._sortObject);
            break;
            default:
            break;
        }

    }

}
