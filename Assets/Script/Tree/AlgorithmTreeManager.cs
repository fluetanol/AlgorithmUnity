using UnityEngine;

public delegate void TraversalDelegate(ref Node node);
public sealed class AlgorithmTreeManager : MonoBehaviour
{
    public GameObject NodePrefab;
    public GameObject ConnectPrefab;
    public static AlgorithmTreeManager Instance;

    public static BinaryTree BTree;
    public static TreeInterface TreeInterface;

    public int InitializeValue = 2;
    private Node _traversalStartNode;
    private float time;


    private TraversalDelegate _traversalDelegate = null;

    // Start is called before the first frame update
    void Awake(){
        Instance = this;
        GameObject rootObject = Instantiate(NodePrefab);
        rootObject.transform.position = Vector3.zero;
        BTree = new BinarySearchTree(rootObject, InitializeValue);
        TreeInterface = BTree;
        _traversalStartNode = BTree.Root;
    }

    void FixedUpdate(){
        time += Time.deltaTime;
        if(time<= 0.5f) return;
        if(_traversalDelegate != null) _traversalDelegate(ref _traversalStartNode);
    }


    public void SetTraversalMode(TraversalDelegate traversalDelegate) => _traversalDelegate = traversalDelegate;
    public void RollBackStartNode() => _traversalStartNode = BTree.Root;
    public void RollBackTime () => time = 0;
}
