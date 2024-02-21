using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SelectionSort : SortInterface
{
    private List<int> _sortList = new();
    private List<GameObject> _sortObject = new();
    //바꿀 위치
    private int _pivotIndex = 0;
    //고른 위치
    private int _minIndex = 0;
    //현재 탐색 위치
    private int _index = 1;

    public AudioSource source;

    public SelectionSort(List<int> sortList, List<GameObject> sortObject){
        _sortList = sortList;
        _sortObject = sortObject;
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

    public bool UpdateSort()
    {
        if (_pivotIndex == _sortList.Count) {
            AlgorithmManager.Instance.source2.pitch = 1;
            return true;
        }
        else if (_index == _sortList.Count)
        {
            ChangeElement(_pivotIndex, _minIndex);
            //AlgorithmManager.Instance.source2.Play();
            //if(AlgorithmManager.Instance.source2.pitch <= 2) AlgorithmManager.Instance.source2.pitch += 0.01f;
            _pivotIndex += 1;
            _index = _pivotIndex + 1;
            _minIndex = _pivotIndex;
        }
        else
        {
            if (_sortList[_index] < _sortList[_minIndex]) _minIndex = _index;
            _index += 1;
        }
        return false;
    }
}
