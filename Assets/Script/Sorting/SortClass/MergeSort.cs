using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : Sort, ISort
{

    public MergeSort(List<SortObject> sortList) : base(sortList){
    }

    private void ChangeElement(int pivotIndex, int changeIndex){
        _sortList[pivotIndex].Set(changeIndex, false);
    }

    public void SetSortList(List<SortObject> sortList)
    {
        SetCollection(sortList);
    }

    public IEnumerator UpdateSort(){
        _isSortFinish = false;
        Queue<List<int>> q = new();
        for(int i=0; i<_sortList.Count; i++){
            List<int> temp = new();
            temp.Add(_sortList[i].value);
            q.Enqueue(temp);
        }

        int idx = 0;
        while(q.Count > 0){
            List<int> left = q.Dequeue();
            q.TryDequeue(out List<int> right);
            List<int> merge = sorting(ref left, ref right);
            
            foreach(int i in merge){
                ChangeElement(idx, i);
                idx++;
                AddTime(0.1f);
                yield return new WaitForSeconds(0.1f);
                if (idx == _sortList.Count) {
                    idx = 0;
                    break;
                }
            }
            if(merge.Count < _sortList.Count){
                q.Enqueue(merge);
            }
            else if(merge.Count == _sortList.Count){
                if(right == null){
                    break;
                }
                else if(left.Count != right.Count){
                    q.Enqueue(merge);
                }
            }  
        }

        _isSortFinish = true;
    }


    private List<int> sorting(ref List<int> left, ref List<int> right){
        List<int> sortList = new();
        if(right ==null){
            return left;
        }

        for(int i=0, j=0; i<left.Count || j<right.Count;){
            if(i >= left.Count){
                sortList.Add(right[j]);
                j++;
            }
            else if(j >= right.Count){
                sortList.Add(left[i]);
                i++;
            }
            else if(left[i] < right[j]){
                sortList.Add(left[i]);
                i++;
            }
            else{
                sortList.Add(right[j]);
                j++;
            }
        }
        return sortList;
    }

}