using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SystemExtension;
using UnityEngine;

public interface ISortInfo{
    float getTime();
    bool  IsSortFinish();
    ESortFlag getSortFlag();
}

public interface ISortControl{
    void StartSort();
    void StopSort();
    void ResetTime();
    void SortForceFinish(); 
    void SetMix(bool isMix);
    bool SelectSort(ESortFlag flag, int size);
}

public class AlgorithmSortingManager : BaseSingleTon<AlgorithmSortingManager>, ISortInfo, ISortControl
{
    public int Size = 10;
    public bool _isMix = true;

    private     IObjectPool<SortObject> _objectPoolInterface;
    private     ISort                   _sortInterface;
    private     Sort                    _sort;
    private     SortFactory             _sortFactory;

    private     List<SortObject>        _sortList = new();
    private     List<int>               _sortListCopy = new();
    private     ESortFlag                sortFlag;
    private     float                   _time;



    void Awake(){
        _objectPoolInterface = FindObjectOfType<SortObjectPool>();
    }

    void Start(){
        ResetSetting(Size);
        _sortFactory   = new SortFactory(_sortList);
        _sortInterface = _sortFactory.GetISort(ESortFlag.Selection);
        _sort          = _sortFactory.GetSort(ESortFlag.Selection);
        sortFlag       = ESortFlag.Selection;
    }

    private void Update() {
        TimeCheck(ref _time);
    }

    void ISortControl.StartSort(){
        StartCoroutine(_sortInterface.UpdateSort());
    }

    void ISortControl.StopSort(){
        StopCoroutine(_sortInterface.UpdateSort());
    }

    public void TimeCheck(ref float time) => time += Time.deltaTime;

    public void ResetSetting(int size){
        _time = 0;
        if(_isMix || Size != size) MixingInitializeSetting(size);
        else if(_isMix && Size == size) MixingFixSetting(size);
        else if(!_isMix){
            for(int i=0; i<_sortList.Count; i++){
                _sortList[i].Set(_sortListCopy[i], false);
            }
        }

        SetCameraSortPosition();
    }

    private void MixingInitializeSetting(int size){
        InitializeList(size);
        InitializeSetSortObject(_sortList);
        RandomizeObject.RandomizeObjectList(ref _sortList, _sortList.Count);
        Size = size;
        _sortListCopy = Enumerable.Range(0, Size)
                        .Select(i => _sortList[i].value).ToList();
        Size = size;
    }


    private void MixingFixSetting(int size){
        RandomizeObject.RandomizeObjectList(ref _sortList, _sortList.Count);
        _sortListCopy = Enumerable.Range(0, Size)
              .Select(i => _sortList[i].value).ToList();
    }


    private void InitializeList(int Size){
        for(int i=0; i<_sortList.Count; i++){
            if(_sortList[i] != null){
                _objectPoolInterface.RemoveObject(_sortList[i]);
            }
        }
        _sortList = Enumerable.Range(0, Size)       // 0 ~ Size-1 까지 Select문의 함수 반복 실행 하는 쿼리
                    .Select(_ => _objectPoolInterface.GetObject())
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
        ResetSetting(size);
        _sortInterface = _sortFactory.GetISort(flag, _sortList);
        return isSuccess;
    }

    public bool IsSortFinish() => _sort.IsSortFinish();
    public float getTime() => _sort.GetSortTime();
    public void ResetTime() => _sort.ResetTime();
    public ESortFlag getSortFlag() => sortFlag;

    public void SortForceFinish(){
        _sort.ForceFinish();
    }

    public void SetMix(bool isMix)
    {
        _isMix = isMix;
    }
}
