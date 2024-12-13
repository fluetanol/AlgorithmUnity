using System.Linq;
using UnityEngine;
using SerializableCollections;

public interface IObjectPool<T>{
    public void RemoveObject(T obj);
    public T GetObject();
}

public class BaseObjectPoolSystem<T> : MonoBehaviour, IObjectPool<T> where T : MonoBehaviour
{
    public GameObject InstantiateObject;
    [SerializeField] protected SerializableQueue<T> _objectList = new();
    [SerializeField] protected Transform parent;


    //BaseObjectPool 스크립트가 부착된 오브젝트 자식으로 풀링할 오브젝트를 두는 것을 추천
    //만약 그렇게 하지 않을 생각이면 반드시 parent를 인스펙터에서 정의
    void Awake()
    {
        if (parent == null){
            parent = transform;
        }
        if (_objectList.Count == 0){
            _objectList = new SerializableQueue<T>(parent.GetComponentsInChildren<T>().ToList());
        }
    }

    public T GetObject(){
        if (_objectList.Count == 0)
        {
            GameObject newObject = Instantiate(InstantiateObject, parent);
            return newObject.GetComponent<T>();
        }
        else{
            T temp = _objectList.Dequeue();
            temp.gameObject.SetActive(true);
            return temp;
        }
    }

    public void RemoveObject(T obj)
    {
        obj.gameObject.SetActive(false);
        _objectList.Enqueue(obj);
    }

}