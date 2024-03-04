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
        levelorder
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
        GameObject rootObject = Instantiate(NodePrefab);
        rootObject.transform.position = Vector3.zero;
        rootObject.GetComponent<NodeObjectInfo>().NodeValueText.text = InitializeValue.ToString();
        BinaryTree = new(rootObject, InitializeValue);
        _traversalStartNode = BinaryTree.Root;
    }

    void FixedUpdate(){
        time += Time.deltaTime;
        if(time<= 0.5f) return;

        switch(_traversalmode){
            case Traversalmode.inorder:
                BinaryTree.UpdateInorderTraversal(ref _traversalStartNode);
                break;
            case Traversalmode.preorder:
                BinaryTree.UpdatePreorderTraversal(ref _traversalStartNode);
                break;

            case Traversalmode.postorder:
                BinaryTree.UpdatePostorderTraversal(ref _traversalStartNode);
                break;

            case Traversalmode.levelorder:
                BinaryTree.UpdateLevelorderTraversal(ref _traversalStartNode);
                break;

        }
    }

    public void SetTraversalMode(Traversalmode traversalmode) => _traversalmode = traversalmode;
    public void RollBackStartNode() => _traversalStartNode = BinaryTree.Root;
    public void RollBackTime () => time = 0;
}
