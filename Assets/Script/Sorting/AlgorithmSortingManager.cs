using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ISortSelect
{
    bool SelectSortMethod(ESortFlag flag, int size);
}

public class SortFactory{
    List<ISortInterface> _sortDictionary;

    public SortFactory(List<SortObject> sortList){
        _sortDictionary = Enumerable.Repeat<ISortInterface>(null, 5).ToList();

        Debug.Log(_sortDictionary.Count);
        _sortDictionary[1] = new SelectionSort(sortList);
        _sortDictionary[2] = new InsertionSort(sortList);
        _sortDictionary[3] = new BubbleSort(sortList);
        _sortDictionary[4] = new MergeSort(sortList);
    }


    public ISortInterface GetSort(ESortFlag flag){
        return _sortDictionary[(int)flag];
    }

    public ISortInterface GetSort(ESortFlag flag, List<SortObject> sortList){
        ISortInterface isort = _sortDictionary[(int)flag];
        isort.SetSortList(sortList);
        return isort;
    }

}

public class AlgorithmSortingManager : MonoBehaviour, ISortSelect
{
    public GameObject SortObject;
    public List<SortObject> _sortList = new();
    public int Size = 50;

    public static AlgorithmSortingManager Instance;

    private ISortInterface _sortInterface;
    private SortFactory _sortFactory;
    private float _time;
    private bool _isFinish;
    private bool _isMix;


    void Awake()=> Instance = this;

    private void OnEnable(){
        InitializeSetting(Size);
    }

    void Start(){ 
        _sortFactory  = new SortFactory(_sortList);
        _sortInterface = _sortFactory.GetSort(ESortFlag.Selection);
    }

    private void FixedUpdate() {
        TimeCheck(ref _time);
        if(!_isFinish) SortingUIManager.Instance.SetTimeText(_time.ToString("0.00"));
        if (_sortInterface.UpdateSort()) {
            SortingUIManager.Instance.SetModeText("Finish!");
            if (!_isFinish)  StartCoroutine(FinishAnimation());     
        }
    }

    IEnumerator FinishAnimation(){
        int i=0;
        _isFinish = true;
        while (i<_sortList.Count){
            Color color = (Color.white/_sortList.Count) * i;
            _sortList[i].SetColor(color);
            i+=1;
            yield return new WaitForSeconds(0.05f);
        }
        if (_isFinish == true)_isFinish = false;
        gameObject.SetActive(false);
        yield break;
    }


    public void TimeCheck(ref float time)=>time += Time.deltaTime;

    public void InitializeSetting(int size){
        _time = 0;
        Size = size;
        InitializeList(size);
        RandomizeObject.RandomizeList(ref _sortList, _sortList.Count);
        InitializeInstantiateObject(_sortList, SortObject);
        SetCameraPosition();
    }

    private void InitializeList(int Size){
        for (int i = 0; i < Size; i++)  _sortList[i].value = i;
    }

    //trash code,.....
    private void InitializeInstantiateObject(List<SortObject> sortList, GameObject InstanceObject)
    {
        foreach(var i in _sortList) Destroy(i);
        _sortList.Clear();

        for (int i = 0; i < sortList.Count; i++)
        {
            var newObject = Instantiate(InstanceObject);
            Vector3 scale = newObject.transform.localScale;
            Vector3 position = newObject.transform.position;
            scale.y = sortList[i].transform.localScale.y;  
            position.x = i;

            newObject.name = sortList[i].ToString();
            newObject.transform.localScale = scale;
            newObject.transform.position = position;
            _sortList.Add(newObject.GetComponent<SortObject>());    
        }
    }

    private void SetCameraPosition(){
        Camera.main.transform.position = new(Size / 2, Size / 2, -10);
        Camera.main.orthographicSize = Size / 1.25f;
    }

    public bool SelectSortMethod(ESortFlag flag, int size)
    {
        bool isSuccess = true;
        InitializeSetting(size);
        if (_isMix) _sortInterface = _sortFactory.GetSort(flag, _sortList);
        else _sortInterface = _sortFactory.GetSort(flag);
        return isSuccess;
    }
}
