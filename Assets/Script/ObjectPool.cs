using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ObjectPoolType{
    Node = 0,
    Edge = 1
}

public class ObjectPool : MonoBehaviour
{
    [Serializable]
    struct ObjectPoolData
    {
        public List<Transform> _poolQueue;
        public GameObject prefab;
    }
    
    
    [SerializeField] private ObjectPoolData _nodePoolData = new();
    [SerializeField] private ObjectPoolData _edgePoolData = new();

    private static GameObject thisObject;

    private static Queue<Transform> s_nodePoolQueue;
    private static Queue<Transform> s_edgePoolQueue;
    private static GameObject s_nodePrefab;
    private static GameObject s_edgePrefab;

    private void Awake(){
        thisObject = gameObject;
        _nodePoolData._poolQueue = new List<Transform>();
        _edgePoolData._poolQueue = new List<Transform>();

        foreach (Transform obj in transform){
            if(obj.TryGetComponent(out Node node)){
                _nodePoolData._poolQueue.Add(obj);
            }
            else if(obj.TryGetComponent(out Edge edge)){
                _edgePoolData._poolQueue.Add(obj);
            }
        }

        s_nodePoolQueue = new Queue<Transform>(_nodePoolData._poolQueue);
        s_nodePrefab = _nodePoolData.prefab;

        s_edgePoolQueue = new Queue<Transform>(_edgePoolData._poolQueue);
        s_edgePrefab = _edgePoolData.prefab;
    }

    public static T GetPoolObject<T>(ObjectPoolType type) where T : Component{
        var (q, prefab) = minifactory(type);
    
        if(q.Count == 0) {
            GameObject obj = Instantiate(prefab);
            return obj.GetComponent<T>();
        }
        else{
            GameObject obj = q.Dequeue().gameObject;
            obj.SetActive(true);
            return obj.GetComponent<T>();
        }
    }

    public static void DestoyPoolObject(GameObject obj, ObjectPoolType type){
        var (q, _) = minifactory(type);
        obj.transform.parent = thisObject.transform;
        q.Enqueue(obj.transform);
        obj.SetActive(false);
    }

    public static void DestoyPoolObject(Transform obj, ObjectPoolType type){
        DestoyPoolObject(obj.gameObject, type);
    }

    private static (Queue<Transform>, GameObject) minifactory(ObjectPoolType type)
    {
        Queue<Transform> q = type switch
        {
            ObjectPoolType.Node => s_nodePoolQueue,
            ObjectPoolType.Edge => s_edgePoolQueue,
            _ => throw new ArgumentException("Invalid type")
        };

        GameObject prefab = type switch
        {
            ObjectPoolType.Node => s_nodePrefab,
            ObjectPoolType.Edge => s_edgePrefab,
            _ => throw new ArgumentException("Invalid type")
        };

        return (q, prefab);
    }



}
