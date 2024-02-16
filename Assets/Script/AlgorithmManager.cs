using System.Collections.Generic;
using UnityEngine;

public class AlgorithmManager : MonoBehaviour
{
    public SortInterface sortInterface;
    public GameObject SortObject;
    public List<int> _sortList = new();
    public List<GameObject> _sortObject = new();
    public int Size = 50;


    private float _time;
    private List<int> _saveList;
    private List<GameObject> _saveObject;


    private void OnEnable()
    {
        InitializeList(Size);
        RandomizeObject.RandomizeList(ref _sortList, _sortList.Count);
        InitializeInstantiateObject(_sortList, SortObject);
        SetCameraPosition();
        
    }
    void Start(){ 
        _saveList = _sortList;
        _saveObject = _sortObject;
        sortInterface = new SelectionSort(_sortList, _sortObject);
    }

    void Update() {
        if(sortInterface.UpdateSort()) {
            print(_time);
            gameObject.SetActive(false);
        }
        TimeCheck(ref _time);
    }

    public void TimeCheck(ref float time)=>time += Time.deltaTime;

    private void InitializeList(int Size){
        for (int i = 0; i < Size; i++)  _sortList.Add(i);
    }

    private void InitializeInstantiateObject(List<int> sortList, GameObject InstanceObject)
    {
        for (int i = 0; i < sortList.Count; i++)
        {
            var newObject = Instantiate(InstanceObject);
            Vector3 scale = newObject.transform.localScale;
            Vector3 position = newObject.transform.position;
            scale.y = sortList[i];
            position.x = i;

            newObject.name = sortList[i].ToString();
            newObject.transform.localScale = scale;
            newObject.transform.position = position;
            _sortObject.Add(newObject);
        }
    }

    private void SetCameraPosition(){
        Camera.main.transform.position = new(Size / 2, Size / 2, -10);
        Camera.main.orthographicSize = Size / 1.5f;
    }
}
