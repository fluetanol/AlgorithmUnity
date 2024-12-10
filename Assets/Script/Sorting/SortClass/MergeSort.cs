using System;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : Sort, ISortInterface
{
    private List<(Vector3, String)> _sortObjectScale = new();
    private List<SortObject> _swapList1 = new();
    private List<SortObject> _swapList2 = new();

    private int _index=0;
    private int _checkIndex = 1;
    private int _length = 1;
    private int _count;
    private int _checkCount;
    private int _listLength;
    private bool _isSwap;

    public MergeSort(List<SortObject> sortList) : base(sortList){
        foreach(var i in _sortList){
            _swapList2.Add(i);
        }
        foreach(var i in sortList){
            _sortObjectScale.Add((i.transform.localScale, i.name));
        }
        _sortList = sortList;
        _listLength = _sortList.Count;
    }

    private void ChangeElement(int pivotIndex, int changeIndex, List<SortObject> tempList){
        _sortList[pivotIndex] = tempList[changeIndex];
        _sortList[pivotIndex].name = _sortObjectScale[changeIndex].Item2;
        _sortList[pivotIndex].transform.localScale = _sortObjectScale[changeIndex].Item1;
    }

    public bool UpdateSort()
    {
        if (_length >= _listLength)  return true;
        if(!_isSwap) Merging(ref _swapList1, ref _swapList2);
        else Merging(ref _swapList2, ref _swapList1);   
        return false;
    }

    private void Merging(ref List<SortObject> swapList1, ref List<SortObject> swapList2){
        //Debug.Log(_index +" "+_checkIndex + " "+_length+ " "+swapList1);
        if (swapList1.Count == swapList2.Count)       //한 주기가 끝나는 타이밍
        {
            swapList2.Clear();
            _sortObjectScale.Clear();
            _length *= 2;
            _index = 0;
            _count = 0;
            _checkCount = 0;
            _checkIndex = _index + _length;
            _isSwap = !_isSwap;
            foreach (var i in _sortList) _sortObjectScale.Add((i.transform.localScale, i.name));
        }

        else if (_count == _length && _checkCount == _length)   //다음 그룹 짝짓는 타이밍
        {
            _index = _index - _length;
            _index += _length * 2;
            _checkIndex = _index + _length;
            _count = 0;
            _checkCount = 0;

            if(_checkIndex>=_listLength){
                Debug.Log("length out, No Group");
            }
        }
        else if (_count >= _length)
        {
            ChangeElement(swapList1.Count , _checkIndex, swapList2);
            AddList(ref swapList1, ref swapList2, ref _checkIndex,ref _checkCount);

        }
        else if (_checkCount >= _length)
        {
            ChangeElement(swapList1.Count, _index, swapList2);
            AddList(ref swapList1, ref swapList2, ref _index, ref _count);

        }
        else if (_checkIndex >= _listLength)
        {
            ChangeElement(swapList1.Count, _index, swapList2);
            AddList(ref swapList1, ref swapList2, ref _index, ref _count);
        }

        else if (swapList2[_index] < swapList2[_checkIndex])
        {
            ChangeElement(swapList1.Count, _index, swapList2);
            AddList(ref swapList1, ref swapList2, ref _index, ref _count);

        }
        else if (swapList2[_index] >= swapList2[_checkIndex])
        {
            ChangeElement(swapList1.Count, _checkIndex, swapList2);
            AddList(ref swapList1, ref swapList2, ref _checkIndex, ref _checkCount);

        }
    }

    private void AddList(ref List<SortObject> swapList1, ref List<SortObject> swapList2, ref int idx, ref int cnt){
        swapList1.Add(swapList2[idx]);
        idx += 1;
        cnt += 1;
    }

    public void SetSortList(List<SortObject> sortList)
    {
        SetCollection(sortList);
    }
}
