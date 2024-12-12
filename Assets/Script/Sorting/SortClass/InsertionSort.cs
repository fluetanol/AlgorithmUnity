using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : Sort, ISortInterface
{
    private int _index=0;
    private int _checkIndex=0; 
    private int _pivotIndex=0;

    public InsertionSort(List<SortObject> sortList):base(sortList){}

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

        //_sortObject[pivotIndex].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    public bool UpdateSort()
    {
        if(_checkIndex == _sortList.Count) {
            return true;
        }
        else if(_index == -1 || _sortList[_index]< _sortList[_checkIndex]){
            _pivotIndex += 1;
            _checkIndex = _pivotIndex;
            _index = _pivotIndex - 1;
            
        }
        else if(_sortList[_index]>=_sortList[_checkIndex]){
            //AlgorithmManager.Instance.source2.Play();
            ChangeElement(_index, _checkIndex);
            _checkIndex -= 1;
            _index -=1;
        }
        return false;
    }

    public void SetSortList(List<SortObject> sortList)
    {
        SetCollection(sortList);
    }
}
