using System.Collections.Generic;
using UnityEngine;

public class Sort
{
    protected List<SortObject> _sortList = new();

    public Sort(List<SortObject> sortList){
        _sortList = sortList;
    }

    public void SetCollection(List<SortObject> sortList){
        _sortList = sortList;
    }

}
