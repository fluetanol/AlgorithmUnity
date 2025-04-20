using System;
using System.Collections;
using UnityEngine;

public delegate void TraversalDelegate(ref Node node);

[Serializable]
public enum TraversalMode{
    PreOrder = 0,
    PostOrder = 1,
    InOrder = 2,
    LevelOrder = 3
 
}


public sealed class AlgorithmTreeManager : MonoBehaviour, INodeManage, ITreeManage, ITreeTraversal{
    [SerializeField] private GameObject       _nodePrefab;
    [SerializeField] private GameObject       _connectPrefab;
    [SerializeField] private float            _perSec;    

    public static            GameObject       NodePrefab;
    public static            GameObject       ConnectPrefab;
    public                   int              InitializeValue = 2;


    private static           BinaryTree         BTree;
    private static           Node              _traversalStartNode;
    
    public static IEnumerator enumerateTraversal;
    private Coroutine _traversalCoroutine;

    // Start is called before the first frame update
    void Awake(){
        NodePrefab = _nodePrefab;
        ConnectPrefab = _connectPrefab;

        Node rootNode = ObjectPool.GetPoolObject<Node>(ObjectPoolType.Node);
        rootNode.transform.position = Vector3.zero;
        BTree = new BinarySearchTree(ref rootNode, InitializeValue);
        _traversalStartNode = BTree.Root;

        enumerateTraversal = BTree.CoroutineInorderTraversal(_traversalStartNode, _perSec);
    }


    public static void RollBackStartNode()          => _traversalStartNode = BTree.Root;
    public void ResetRecentNode()                   => BTree.ResetRecentNode();
    public void SetRootValue(int value)             => BTree.SetRootValue(value);

    public void SetNewTree(int num)
    {
        SetTraversalMode(null);
        if (BTree.Root.right != null)
        {
            ObjectPool.DestoyPoolObject(BTree.Root.right.gameObject, ObjectPoolType.Node);
            //Destroy(BTree.Root.right.gameObject);
            //Destroy(BTree.Root.right.ConnectObject);
        }
        if (BTree.Root.left != null)
        {
            ObjectPool.DestoyPoolObject(BTree.Root.left.gameObject, ObjectPoolType.Node);
            //Destroy(BTree.Root.left.gameObject);
            //Destroy(BTree.Root.left.ConnectObject);
        }
        BTree.Root.right = null;
        BTree.Root.left = null;

        Node node = BTree.Root;

        if (num == 0) BTree = new BinarySearchTree(ref node, 0);
        else if (num == 1) BTree = new AVLTree(node, 0);

        RollBackStartNode();
    }


    public Node NewNode(int value){
        Node node = ObjectPool.GetPoolObject<Node>(ObjectPoolType.Node);
        node.Depth = 0;
        node.SetNodeValue(value);
        return node;
    }

    public Edge NewEdge(){
        Edge edge = ObjectPool.GetPoolObject<Edge>(ObjectPoolType.Edge);
        return edge;
    }

    public bool AddNode(Node node, Edge edge){
        bool k = BTree.Add(node, edge);
        if(k) BTree.NodeMoveAnimation(node, 0.5f);
        return k;
    }
    public GameObject RemoveNode(int value){
        GameObject g = BTree.Remove(value);
        if(g!=null) BTree.NodeMoveAnimation(BTree.Root, 0.5f);
        return g;
    }
    public bool IsExistNode(int value, out Node node)     => BTree.isExist(value, out node);
    public int  GetTreeNodeCount()                        => BTree.GetNodeCount();


    public void SetTraversalMode(TraversalMode? mode){
        if(mode == null) return;
        switch(mode){
            case TraversalMode.InOrder:
                enumerateTraversal = BTree.CoroutineInorderTraversal(_traversalStartNode, _perSec);;
                break;
            case TraversalMode.PreOrder:
                enumerateTraversal = BTree.CoroutinePreorderTraversal(_traversalStartNode, _perSec);
                break;
            case TraversalMode.PostOrder:
                enumerateTraversal = BTree.CoroutinePostorderTraversal(_traversalStartNode, _perSec);
                break;
            case TraversalMode.LevelOrder:
                enumerateTraversal = BTree.CoroutineLevelorderTraversal(_traversalStartNode, _perSec);
                break;
        }
    }


    public void EnumerateCoroutineTraversal(){
        if(_traversalCoroutine != null){
            StopCoroutine(_traversalCoroutine);
            TraversalReset();
        }
        _traversalCoroutine = StartCoroutine(enumerateTraversal);
    }


    public void EnumerateStepTraversal(){
        if(!enumerateTraversal.MoveNext()){
            TraversalReset();
        }
    }

    private void TraversalReset(){
        ResetRecentNode();
        RollBackStartNode();
        BTree.LevelOrderQueueReset();
    }

    public void Test(){
        Debug.Log("Test");
    }
}
    

