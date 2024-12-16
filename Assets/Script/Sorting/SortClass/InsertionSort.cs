using System.Collections;
using System.Collections.Generic;
using SystemExtension;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InsertionSort : Sort, ISort
{
    public InsertionSort(List<SortObject> sortList):base(sortList){}

    private void ChangeElement(int pivotIndex, int changeIndex){
        SortObject pivotObject =  _sortList[pivotIndex];
       SortObject changeObject =  _sortList[changeIndex];

       ExtensionFunction.SwapGameObject(ref pivotObject, ref changeObject);
        _sortList.SwapElement(pivotIndex, changeIndex);
    }
    
    public void SetSortList(List<SortObject> sortList)
    {
        SetCollection(sortList);
    }

    public IEnumerator UpdateSort()
    {
        _isSortFinish = false;
       for(int i=1; i<_sortList.Count; i++){
            for(int j=i; j>0; j--){
                if(_sortList[j]<_sortList[j-1]){
                    ChangeElement(j, j-1);
                    AddTime(0.1f);
                    yield return new WaitForSeconds(0.1f);
                }
                else{
                    break;
                }
            }
       }
         _isSortFinish = true;
    }
}
