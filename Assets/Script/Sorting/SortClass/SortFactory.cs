using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SortFactory
{
    List<ISort> _sortDictionary;

    public SortFactory(List<SortObject> sortList)
    {
        _sortDictionary = Enumerable.Repeat<ISort>(null, 5).ToList();

        Debug.Log(_sortDictionary.Count);
        _sortDictionary[1] = new SelectionSort(sortList);
        _sortDictionary[2] = new InsertionSort(sortList);
        _sortDictionary[3] = new BubbleSort(sortList);
        _sortDictionary[4] = new MergeSort(sortList);
    }


    public ISort GetSort(ESortFlag flag)
    {
        return _sortDictionary[(int)flag];
    }

    public ISort GetSort(ESortFlag flag, List<SortObject> sortList)
    {
        ISort isort = _sortDictionary[(int)flag];
        isort.SetSortList(sortList);
        return isort;
    }

}
