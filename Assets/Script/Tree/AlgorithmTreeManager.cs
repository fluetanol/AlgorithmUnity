using System;
using UnityEngine;
public class AlgorithmTreeManager : MonoBehaviour
{
    public GameObject NodePrefab;
    public GameObject ConnectPrefab;
    public static AlgorithmTreeManager Instance;
    public static BinaryTree BinaryTree;
    public int InitializeValue = 2;
    private Node _traversalStartNode;
    private float time;

    public delegate void TraversalDelegate(ref Node node);
    private TraversalDelegate _traversalDelegate = null;

    // Start is called before the first frame update
    void Awake(){
        Instance = this;
        GameObject rootObject = Instantiate(NodePrefab);
        rootObject.transform.position = Vector3.zero;
        rootObject.GetComponent<NodeObjectInfo>().NodeValueText.text = InitializeValue.ToString();
        BinaryTree = new(rootObject, InitializeValue);
        _traversalStartNode = BinaryTree.Root;
    }

    void FixedUpdate(){
        time += Time.deltaTime;
        if(time<= 0.5f) return;
        if(_traversalDelegate != null) _traversalDelegate(ref _traversalStartNode);
    }


    public void SetTraversalMode(TraversalDelegate traversalDelegate) => _traversalDelegate = traversalDelegate;
    public void RollBackStartNode() => _traversalStartNode = BinaryTree.Root;
    public void RollBackTime () => time = 0;
}
