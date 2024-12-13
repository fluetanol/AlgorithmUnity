using System.Collections.Generic;
using UnityEngine;

public class SelectionSort : Sort, ISort
{
    //바꿀 위치
    private int _pivotIndex = 0;
    //고른 위치
    private int _minIndex = 0;
    //현재 탐색 위치
    private int _index = 1;
    private Color color;

    public AudioSource source;

    public SelectionSort(List<SortObject> sortList) : base(sortList) { 
        color = _sortList[0].GetColor();
    }

    private void ChangeElement(int pivotIndex, int changeIndex){
        Vector3 pivotObjectScale = _sortList[pivotIndex].transform.localScale;
        string pivotObjectName = _sortList[pivotIndex].name;
        int pivotNum = _sortList[pivotIndex].value;
        _sortList[pivotIndex] = _sortList[changeIndex];
        _sortList[changeIndex].Set(pivotNum);

        _sortList[pivotIndex].name = _sortList[changeIndex].name;
        _sortList[changeIndex].name = pivotObjectName;
        _sortList[pivotIndex].transform.localScale = _sortList[changeIndex].transform.localScale;
        _sortList[changeIndex].transform.localScale = pivotObjectScale;

        //_sortList[_pivotIndex].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    private void ColorChange(int idx, int before, Color idxColor){
        _sortList[idx].GetComponentInChildren<SpriteRenderer>().material.color = idxColor;
        _sortList[before].GetComponentInChildren<SpriteRenderer>().material.color = color;
    }

    public bool UpdateSort()
    {
        if (_pivotIndex == _sortList.Count) {
            return true;
        }
        else if (_index == _sortList.Count)
        {
            ColorChange(_minIndex, _minIndex, Color.clear);
            ChangeElement(_pivotIndex, _minIndex);
            if(_pivotIndex+1<_sortList.Count) ColorChange(_pivotIndex + 1, _pivotIndex, Color.red);
            else ColorChange(_pivotIndex, _pivotIndex, Color.clear);
            _pivotIndex += 1;
            _index = _pivotIndex + 1;
            _minIndex = _pivotIndex;
        }
        else
        {
            if (_sortList[_index] < _sortList[_minIndex]){
                ColorChange(_index, _minIndex, Color.green);
                _minIndex = _index;
            }
            _index += 1;
        }
        return false;
    }

    public void SetSortList(List<SortObject> sortList)
    {
        SetCollection(sortList);
        _pivotIndex = 0;
    }
}
