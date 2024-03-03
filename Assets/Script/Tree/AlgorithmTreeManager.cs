using System;
using UnityEngine;



public class AlgorithmTreeManager : MonoBehaviour
{
    public enum Traversalmode
    {
        None,
        preorder,
        postorder,
        inorder,
    }

    public GameObject NodePrefab;
    public GameObject ConnectPrefab;
    public static AlgorithmTreeManager Instance;
    public static BinaryTree BinaryTree;
    public int InitializeValue = 2;

    private Traversalmode _traversalmode;
    private Node _traversalStartNode;
    private float time;

    // Start is called before the first frame update
    void Awake(){
        Instance = this;
        BinaryTree = new();
        GameObject rootObject = Instantiate(NodePrefab);
        rootObject.transform.position = Vector3.zero;
        rootObject.GetComponent<NodeObjectInfo>().NodeValueText.text = InitializeValue.ToString();
        BinaryTree.Root.NodeObject = rootObject;
        BinaryTree.Root.Value = InitializeValue;
        _traversalStartNode = BinaryTree.Root;
    }

    void FixedUpdate(){
        time += Time.deltaTime;
        if(time<= 0.5f) return;

        if(_traversalmode == Traversalmode.inorder)    BinaryTree.UpdateInorderTraversal(ref _traversalStartNode);
        else if (_traversalmode == Traversalmode.preorder) BinaryTree.UpdatePreorderTraversal(ref _traversalStartNode);
        else if (_traversalmode == Traversalmode.postorder) BinaryTree.UpdatePostorderTraversal(ref _traversalStartNode);
    }

    public void SetTraversalMode(Traversalmode traversalmode) => _traversalmode = traversalmode;
    public void RollBackStartNode() => _traversalStartNode = BinaryTree.Root;
    public void RollBackTime () => time = 0;
}
