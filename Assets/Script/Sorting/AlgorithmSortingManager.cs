using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SystemExtension;
using Unity.Mathematics;
using UnityEngine;

public interface ISortSelect
{
    bool SelectSort(ESortFlag flag, int size);
}

public class AlgorithmSortingManager : BaseSingleTon<AlgorithmSortingManager>, ISortSelect
{
    public List<SortObject> _sortList = new();
    public int Size = 10;

    private IObjectPool<SortObject> _objectPool;
    private ISort _sortInterface;
    private SortFactory _sortFactory;
    private float _time;
    private bool _isFinish;
    private bool _isMix;


    void Awake(){
        _objectPool = FindObjectOfType<SortObjectPool>();
    }

    void Start(){
        InitializeSetting(Size);
        _sortFactory = new SortFactory(_sortList);
        _sortInterface = _sortFactory.GetSort(ESortFlag.Selection);
    }
   

    private void FixedUpdate() {
        /*
        TimeCheck(ref _time);
        if(!_isFinish) SortingUIManager.Instance.SetTimeText(_time.ToString("0.00"));
        if (_sortInterface.UpdateSort()) {
            SortingUIManager.Instance.SetModeText("Finish!");       
            if (!_isFinish)  StartCoroutine(FinishAnimation());     
        }*/
    }

    /*
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
    }*/


    public void TimeCheck(ref float time)=>time += Time.deltaTime;

    public void InitializeSetting(int size){
        _time = 0;
        Size = size;
        InitializeList(size);
        InitializeSetSortObject(_sortList);
        RandomizeObject.RandomizeObjectList(ref _sortList, _sortList.Count);
        SetCameraSortPosition();
    }

    private void InitializeList(int Size){
        foreach(var sortObject in _sortList){
            _objectPool.RemoveObject(sortObject);
        }

        _sortList = Enumerable.Range(0, Size)
                    .Select(_ => _objectPool.GetObject())
                    .ToList(); 
    }

    private void InitializeSetSortObject(List<SortObject> sortList){
        for (int i = 1; i <= sortList.Count; i++){
            _sortList[i-1].Set(i);
        }
    }

    private void SetCameraSortPosition(){
        Vector3 middlePosition = _sortList[_sortList.Count / 2].transform.position;
        int size = _sortList[_sortList.Count / 2].value;

        Camera.main.transform.position = new(middlePosition.x, Size / 2f, -10);
        Camera.main.orthographicSize = Size / 1.25f;
    }

    public bool SelectSort(ESortFlag flag, int size){
        bool isSuccess = true;
        InitializeSetting(size);
        if (_isMix) _sortInterface = _sortFactory.GetSort(flag, _sortList);
        else _sortInterface = _sortFactory.GetSort(flag);
        return isSuccess;
    }
}
