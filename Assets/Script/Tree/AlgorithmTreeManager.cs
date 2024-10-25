using UnityEngine;

public delegate void TraversalDelegate(ref Node node);

public sealed class AlgorithmTreeManager : MonoBehaviour, INodeManage, ITreeManage, ITreeTraversal{
    [SerializeField] private GameObject       _nodePrefab;
    [SerializeField] private GameObject       _connectPrefab;

    public static            GameObject       NodePrefab;
    public static            GameObject       ConnectPrefab;
    public                   int              InitializeValue = 2;


    private static           BinaryTree         BTree;
    private static           Node              _traversalStartNode;
    private static           TraversalDelegate _traversalDelegate = null;
    private static           float             time;
    

    // Start is called before the first frame update
    void Awake(){
        NodePrefab = _nodePrefab;
        ConnectPrefab = _connectPrefab;

        GameObject rootObject = Instantiate(NodePrefab);
        rootObject.transform.position = Vector3.zero;
        BTree = new BinarySearchTree(rootObject, InitializeValue);
        _traversalStartNode = BTree.Root;
    }

    void FixedUpdate(){
        time += Time.deltaTime;
        if(time<= 0.5f) return;
        if(_traversalDelegate != null) _traversalDelegate(ref _traversalStartNode);
    }

    public static void SetTraversalMode(TraversalDelegate traversalDelegate) => _traversalDelegate = traversalDelegate;
    public static void RollBackStartNode()                                   => _traversalStartNode = BTree.Root;
    public static void RollBackTime()                                        => time = 0;



    public void ResetRecentNode() => BTree.ResetRecentNode();
    public void SetRootValue(int value) => BTree.SetRootValue(value);
    public void SetNewTree(int num)
    {
        SetTraversalMode(null);
        if (BTree.Root.right != null)
        {
            Destroy(BTree.Root.right.NodeObject);
            Destroy(BTree.Root.right.ConnectObject);
        }
        if (BTree.Root.left != null)
        {
            Destroy(BTree.Root.left.NodeObject);
            Destroy(BTree.Root.left.ConnectObject);
        }
        BTree.Root.right = null;
        BTree.Root.left = null;

        Node node = BTree.Root;

        if (num == 0) BTree = new BinarySearchTree(node.NodeObject, 0);
        else if (num == 1) BTree = new AVLTree(node.NodeObject, 0);

        RollBackStartNode();
    }


    public Node NewNode(int value){
        GameObject NodeObject = Instantiate(_nodePrefab);
        GameObject ConnectObject = Instantiate(_connectPrefab);
        Node node = new Node()
        {
            Value = value,
            NodeObject = NodeObject,
            ConnectObject = ConnectObject
        };
        return node;
    }


    public bool AddNode(Node node)                        => BTree.Add(node);
    public (GameObject, GameObject) RemoveNode(int value) => BTree.Remove(value);
    public bool IsExistNode(int value)                    => BTree.isExist(value);
    public int  GetTreeNodeCount()                        => BTree.GetNodeCount();
    public void InOrderTraversal()                        => BTree.InorderTraversal();
    public void PreOrderTraversal()                       => BTree.PreorderTraversal();
    public void PostOrderTraversal()                      => BTree.PostOrderTraversal();
    public void LevelOrderTraversal()                     => BTree.LevelorderTraversal();


    public void UpdateInorderTraversal(ref Node node)     => BTree.UpdateInorderTraversal(ref node);
    public void UpdatePreorderTraversal(ref Node node)    => BTree.UpdatePreorderTraversal(ref node);
    public void UpdatePostorderTraversal(ref Node node)   => BTree.UpdatePostorderTraversal(ref node);
    public void UpdateLevelorderTraversal(ref Node node)  => BTree.UpdateLevelorderTraversal(ref node);

}
    

