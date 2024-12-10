using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : Sort, ISortInterface
{

    private int _index=0;
    private int _checkIndex=1;
    private int _pivotIndex=0;

    public BubbleSort(List<int> sortList, List<GameObject> sortObject){
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
    }

    public bool UpdateSort()
    {
        //Debug.Log(_pivotIndex + " "+_index + " "+ _checkIndex);

        if (_pivotIndex >= _sortList.Count - 1) {
            //AlgorithmManager.Instance.source2.pitch = 1;
            return true;
        }
        if (_checkIndex >= _sortList.Count - _pivotIndex) {
            //_sortObject[_checkIndex-1].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            _checkIndex = 1;
            _index = 0;
            _pivotIndex += 1;
        }

        if (_sortList[_index] > _sortList[_checkIndex]) {
            ChangeElement(_index, _checkIndex);
            //AlgorithmManager.Instance.source2.Play();
        }

        _index += 1;
        _checkIndex += 1;

        return false;
    }

    public void SetSortList(List<int> sortList, List<GameObject> sortObject)
    {
        SetCollection(sortList, sortObject);
    }
}
