using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : Sort, ISort
{

    private int _index=0;
    private int _checkIndex=1;
    private int _pivotIndex=0;

    public BubbleSort(List<SortObject> sortList):base(sortList){}

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

    public void SetSortList(List<SortObject> sortList)
    {
        SetCollection(sortList);
    }
}
