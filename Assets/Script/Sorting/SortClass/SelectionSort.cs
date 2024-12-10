using System.Collections.Generic;
using UnityEngine;

public class SelectionSort : Sort, ISortInterface
{
    //바꿀 위치
    private int _pivotIndex = 0;
    //고른 위치
    private int _minIndex = 0;
    //현재 탐색 위치
    private int _index = 1;
    private Color color;

    public AudioSource source;

    public SelectionSort(List<int> sortList, List<GameObject> sortObject){
        _sortList = sortList;
        _sortObject = sortObject;
        color = _sortObject[0].GetComponentInChildren<SpriteRenderer>().material.color;
    }

    private void ChangeElement(int pivotIndex, int changeIndex){
        Vector3 pivotObjectScale = _sortObject[pivotIndex].transform.localScale;
        string pivotObjectName = _sortObject[pivotIndex].name;
        int pivotNum = _sortList[pivotIndex];
        _sortList[pivotIndex] = _sortList[changeIndex];
        _sortList[changeIndex] = pivotNum;

        _sortObject[pivotIndex].name = _sortObject[changeIndex].name;
        _sortObject[changeIndex].name = pivotObjectName;
        _sortObject[pivotIndex].transform.localScale = _sortObject[changeIndex].transform.localScale;
        _sortObject[changeIndex].transform.localScale = pivotObjectScale;

        //_sortObject[_pivotIndex].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    private void ColorChange(int idx, int before, Color idxColor){
        _sortObject[idx].GetComponentInChildren<SpriteRenderer>().material.color = idxColor;
        _sortObject[before].GetComponentInChildren<SpriteRenderer>().material.color = color;
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

    public void SetSortList(List<int> sortList, List<GameObject> sortObject)
    {
        SetCollection(sortList, sortObject);
        _pivotIndex = 0;
    }
}
