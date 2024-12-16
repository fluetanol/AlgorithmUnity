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

    public void ForceFinish(){
        _isSortFinish = true;
        ResetTime();
    }

    public float GetSortTime(){
        return _sortTime;
    }
    
    public void ResetTime(){
        _sortTime = 0f;
    }

    public void AddTime(float deltaTime){
        _sortTime += deltaTime;
    }

    
}
