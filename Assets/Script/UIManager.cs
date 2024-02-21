using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum SortFlag
{
    Insertion,
    Selection
}

public class UIManager : MonoBehaviour
{
    public TMP_Text TimeText;
    public TMP_Text ModeText;
    public TMP_InputField CountInputField;
    public static UIManager Instance;

    void Awake() => Instance = this;
    void OnEnable() => DontDestroyOnLoad(gameObject);

    public void SetTimeText(string text) => TimeText.text = text;
    public void SetModeText(string text) => ModeText.text = text;

    public void SetSort(int flag)
    {
        if(!AlgorithmManager.Instance.gameObject.activeSelf) AlgorithmManager.Instance.gameObject.SetActive(true);
        if(Int32.TryParse(CountInputField.text, out int result)) AlgorithmManager.Instance.InitializeSetting(result);
        else AlgorithmManager.Instance.InitializeSetting(10);

        switch(flag){
            case 0:
            ModeText.text = "Sort: Selection";
            AlgorithmManager._sortInterface = new SelectionSort(AlgorithmManager.Instance._sortList, AlgorithmManager.Instance._sortObject);
            break;
            
            case 1:
            ModeText.text = "Sort: Insertion";
            AlgorithmManager._sortInterface = new InsertionSort(AlgorithmManager.Instance._sortList, AlgorithmManager.Instance._sortObject);
            break;

            case 2:
            ModeText.text = "Sort: Bubble";
            AlgorithmManager._sortInterface = new BubbleSort(AlgorithmManager.Instance._sortList, AlgorithmManager.Instance._sortObject);
            break;

            case 3:
            ModeText.text = "Sort: Merge";
            AlgorithmManager._sortInterface = new MergeSort(AlgorithmManager.Instance._sortList, AlgorithmManager.Instance._sortObject);
            break;


            default:
            break;
        }

    }

}
