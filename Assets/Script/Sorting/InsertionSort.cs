using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : SortInterface
{
    private List<int> _sortList = new();
    private List<GameObject> _sortObject = new();
    
    private int _index=0;
    private int _checkIndex=0; 
    private int _pivotIndex=0;

    public InsertionSort(List<int> sortList, List<GameObject> sortObject){
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

        _sortObject[pivotIndex].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    public bool UpdateSort()
    {
        if(_checkIndex == _sortList.Count) {
            for(int i=_sortObject.Count-1; i>=0; i--){
                if(_sortObject[i].GetComponentInChildren<MeshRenderer>().material.color == Color.red) break;
                _sortObject[i].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            }
            return true;
        }
        else if(_index == -1 || _sortList[_index]< _sortList[_checkIndex]){
            _pivotIndex += 1;
            _checkIndex = _pivotIndex;
            _index = _pivotIndex - 1;
            
        }
        else if(_sortList[_index]>=_sortList[_checkIndex]){
            ChangeElement(_index, _checkIndex);
            _checkIndex -= 1;
            _index -=1;
        }
        return false;
    }

}
