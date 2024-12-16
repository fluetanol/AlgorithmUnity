using System.Collections.Generic;

public class Sort
{
    protected List<SortObject> _sortList = new();
    protected static bool _isSortFinish = false;
    protected static float _sortTime = 0f;

    public Sort(List<SortObject> sortList){
        _sortList = sortList;
    }

    public void SetCollection(List<SortObject> sortList){
        _sortList = sortList;
    }

    public bool IsSortFinish(){
        return _isSortFinish;
    }

    public float GetSortTime(){
        return _sortTime;
    }

    public void AddTime(float deltaTime){
        _sortTime += deltaTime;
    }

    
}
