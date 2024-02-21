using System.Collections.Generic;
using UnityEngine;

public class MergeSort : SortInterface
{
    private List<int> _sortList = new();
    private List<GameObject> _sortObject = new();
    private List<int> _swapList = new();

    private int _index=0;
    private int _checkIndex = 1;
    private int _length = 1;
    private int _count;
    private int _checkCount;
    private int _listLength;
    private bool _isSwap;

    public MergeSort(List<int> sortList, List<GameObject> sortObject){
        _sortList = sortList;
        _sortObject = sortObject;
        _listLength = _sortList.Count;
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
        if(_length == _listLength) {
           foreach(var k in _swapList){
                _sortList.Add(k);
           }
            return true;
        }

        if(!_isSwap){
            if (_swapList.Count == _sortList.Count){
                _length *= 2;
                _sortList.Clear();
                _index = 0;
                _checkIndex = _index + _length;
                _count = 0;
                _checkCount = 0;
                _isSwap = !_isSwap;
            }
            else if(_count == _length && _checkCount == _length){
                Debug.Log("lvlup!");
                _index = _index-_length;
                _index += _length * 2;
                _checkIndex = _index + _length;
                _count = 0;
                _checkCount=0;
            }
            else if (_count >= _length)
            {
                _swapList.Add(_sortList[_checkIndex]);
                _checkIndex += 1;
                _checkCount+=1;
            }
            else if (_checkCount >= _length)
            {
                _swapList.Add(_sortList[_index]);
                _index += 1;
                _count+=1;
            }
            else if(_sortList[_index] < _sortList[_checkIndex]){ 
                _swapList.Add(_sortList[_index]);  
                _index+=1;
                _count+=1;
           }
           else if(_sortList[_index] >= _sortList[_checkIndex]){
                _swapList.Add(_sortList[_checkIndex]);
                _checkIndex+=1;
                _checkCount+=1;
           }

        }
        else{
            Debug.Log(_length + " " + _index + " " + _checkIndex + " " + _count + " " + _checkCount);
            if (_swapList.Count == _sortList.Count){
                _length *= 2;
                _swapList.Clear();
                _index = 0;
                _checkIndex = _index + _length;
                _count = 0;
                _checkCount = 0;
                _isSwap = !_isSwap;
            }
            else if (_count == _length && _checkCount == _length)
            {
                _index = _index - _length;
                _index += _length * 2;
                _checkIndex = _index + _length;
                _count=0;
                _checkCount = 0;
            }
            else if (_count >= _length)
            {
                _sortList.Add(_swapList[_checkIndex]);
                _checkIndex += 1;
                _checkCount += 1;
            }
            else if (_checkCount >= _length)
            {
                _sortList.Add(_swapList[_index]);
                _index += 1;
                _count += 1;
            }
            else if (_swapList[_index] < _swapList[_checkIndex]) {
                _sortList.Add(_swapList[_index]);
                _index += 1;
                _count += 1;
            }
            else if (_swapList[_index] >= _swapList[_checkIndex])
            {
                _sortList.Add(_swapList[_checkIndex]);
                _checkIndex += 1;
                _checkCount += 1;
            }
        }
        

        return false;
    }

}
