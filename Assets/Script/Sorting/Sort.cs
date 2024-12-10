using System.Collections.Generic;
using UnityEngine;

public class Sort
{
    protected List<int> _sortList = new();
    protected List<GameObject> _sortObject = new();

    public void SetCollection(List<int> sortList, List<GameObject> sortObject){
        _sortList = sortList;
        _sortObject = sortObject;
    }

}
