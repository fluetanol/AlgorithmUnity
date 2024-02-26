using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmSortingManager : MonoBehaviour
{
    public GameObject SortObject;
    public List<int> _sortList = new();
    public List<GameObject> _sortObject = new();
    public int Size = 50;
    //public AudioSource source;
    //public AudioSource source2;

    public static AlgorithmSortingManager Instance;
    public static SortInterface _sortInterface;

    private float _time;
    private bool _isFinish;
    void Awake()=> Instance = this;

    private void OnEnable(){
        InitializeSetting(Size);
        DontDestroyOnLoad(this);
    }

    void Start(){ 
        _sortInterface = new SelectionSort(_sortList, _sortObject);
    }

    private void FixedUpdate() {
        TimeCheck(ref _time);
        if(!_isFinish) UIManager.Instance.SetTimeText("Time : " + _time.ToString());
        if (_sortInterface.UpdateSort()) {
            UIManager.Instance.SetModeText("Finish!");
            if (!_isFinish)  StartCoroutine(FinishAnimation());     
        }
    }

    IEnumerator FinishAnimation(){
        int i=0;
        _isFinish = true;
        while (i<_sortObject.Count){
            Color color = (Color.white/_sortObject.Count) * i;
            _sortObject[i].GetComponentInChildren<MeshRenderer>().material.color = color;
            i+=1;
            yield return new WaitForSeconds(0.05f);
        }
        if (_isFinish == true)_isFinish = false;
        gameObject.SetActive(false);
        yield break;
    }


    public void TimeCheck(ref float time)=>time += Time.deltaTime;

    public void InitializeSetting(int size){
        _time = 0;
        Size = size;
        InitializeList(size);
        RandomizeObject.RandomizeList(ref _sortList, _sortList.Count);
        InitializeInstantiateObject(_sortList, SortObject);
        SetCameraPosition();
    }

    private void InitializeList(int Size){
        _sortList.Clear();
        for (int i = 0; i < Size; i++)  _sortList.Add(i+1);
    }

    private void InitializeInstantiateObject(List<int> sortList, GameObject InstanceObject)
    {
        foreach(var i in _sortObject) Destroy(i);
        _sortObject.Clear();

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
