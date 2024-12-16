using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using SystemExtension;

public class SelectionSort : Sort, ISort
{
    private Color color;
    public AudioSource source;

    public SelectionSort(List<SortObject> sortList) : base(sortList) { 
        color = _sortList[0].GetColor();
    }

    private void ChangeElement(int pivotIndex, int changeIndex){
        SortObject pivotObject =  _sortList[pivotIndex];
       SortObject changeObject =  _sortList[changeIndex];

       
       _sortList.SwapElement(pivotIndex, changeIndex);
       ExtensionFunction.SwapGameObject(ref pivotObject, ref changeObject);
    
    }

    private void ColorChange(int idx, int before, Color idxColor){
        _sortList[idx].GetComponentInChildren<SpriteRenderer>().material.color = idxColor;
        _sortList[before].GetComponentInChildren<SpriteRenderer>().material.color = color;
    }



    public IEnumerator UpdateSort(){
        _isSortFinish = false;
        for(int i=0; i<_sortList.Count; i++){
            int min = _sortList[i].value;
            int minidx = i;
            for(int j=i; j<_sortList.Count; j++){
                if(min > _sortList[j].value) {
                    min = _sortList[j].value;
                    minidx = j;
                }
            }
            ChangeElement(i, minidx);
            AddTime(0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        _isSortFinish = true;
    }

    public void SetSortList(List<SortObject> sortList){
        SetCollection(sortList);
    }
}
