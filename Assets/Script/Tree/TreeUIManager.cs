using TMPro;
using UnityEngine;

public class TreeUIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private GameObject _nodeInfoPrefab;
    [SerializeField] private Transform _nodeInfoParent;
    [SerializeField] private Transform _traversalOption;

    private static GameObject _staticNodeInfoPrefab;
    private static Transform _staticNodeInfoParent;
    private int _traversalOptionNum = 0;

    private void OnEnable() {
        _staticNodeInfoPrefab = _nodeInfoPrefab;
        _staticNodeInfoParent = _nodeInfoParent;
    }


    public void SetNewTree(int num){
        AlgorithmTreeManager.Instance.SetTraversalMode(null);
        if(AlgorithmTreeManager.BTree.Root.right!=null){
            Destroy(AlgorithmTreeManager.BTree.Root.right.NodeObject);
            Destroy(AlgorithmTreeManager.BTree.Root.right.ConnectObject);
        }
        if (AlgorithmTreeManager.BTree.Root.left != null){
            Destroy(AlgorithmTreeManager.BTree.Root.left.NodeObject);
            Destroy(AlgorithmTreeManager.BTree.Root.left.ConnectObject);
        } 
        AlgorithmTreeManager.BTree.Root.right = null;
        AlgorithmTreeManager.BTree.Root.left = null;

        Node node = AlgorithmTreeManager.BTree.Root;

        if (num==0)  AlgorithmTreeManager.BTree = new BinarySearchTree(node.NodeObject, 0);
        else if(num==1) AlgorithmTreeManager.BTree = new AVLTree(node.NodeObject, 0);

        AlgorithmTreeManager.TreeInterface = AlgorithmTreeManager.BTree;
        AlgorithmTreeManager.Instance.RollBackStartNode();
    }


    public void AddNode(){
        if(int.TryParse(inputField.text, out int value)){
            GameObject NodeObject = Instantiate(AlgorithmTreeManager.Instance.NodePrefab);
            GameObject ConnectObject = Instantiate(AlgorithmTreeManager.Instance.ConnectPrefab);
            Node node = new Node(){
                Value = value,
                NodeObject = NodeObject,
                ConnectObject = ConnectObject
            };
            if(AlgorithmTreeManager.BTree.Add(node)){
                textField.text = "Success Add Node :" + value;
            }
            else{
                textField.text = "Fail Add Node :" + value;
                Destroy(NodeObject);
                Destroy(ConnectObject);
            }

        }
        else textField.text = "Wrong Num";
        
    }
    public void FindNode()
    {
        if (int.TryParse(inputField.text, out int value))
        {
            if(AlgorithmTreeManager.BTree.isExist(value)){
                textField.text = "Find!";
            }
            else textField.text = "NotFound";
        }
        else textField.text = "Wrong Num";
    }

    public void RemoveNode(){
        if (int.TryParse(inputField.text, out int value))
        {
            (GameObject, GameObject) RemoveObject = AlgorithmTreeManager.BTree.Remove(value);
            if (RemoveObject.Item1 == null) textField.text = "NotFound";
            else {
                Destroy(RemoveObject.Item1);
                Destroy(RemoveObject.Item2);
                textField.text = "Remove!";
            }
        }
    }

    public void SetRootNodeValue(){
        if (int.TryParse(inputField.text, out int value) && AlgorithmTreeManager.BTree.GetNodeCount() == 1){
            AlgorithmTreeManager.BTree.SetRootValue(value);
        }
    }

    public void PanelNumberMove(bool reverse){
        _traversalOption.GetChild(_traversalOptionNum).gameObject.SetActive(false);
        if (reverse) {
            _traversalOptionNum = (_traversalOptionNum - 1);
            if(_traversalOptionNum<0) _traversalOptionNum = 3;
        }
        else _traversalOptionNum = (_traversalOptionNum+1)%4;
        _traversalOption.GetChild(_traversalOptionNum).gameObject.SetActive(true);
    }

    public void PreorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.TreeInterface.PreorderTraversal();
    }

    public void InorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.TreeInterface.InorderTraversal();
    }

    public void PostorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.TreeInterface.PostOrderTraversal();
    }

    public void LevelorderTraversal()
    {
        TraversalReset();
        AlgorithmTreeManager.TreeInterface.LevelorderTraversal();
    }

    public void UpdateInorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.Instance.SetTraversalMode(new TraversalDelegate(AlgorithmTreeManager.TreeInterface.UpdateInorderTraversal));
    }

    public void UpdatePreorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.Instance.SetTraversalMode(new TraversalDelegate(AlgorithmTreeManager.TreeInterface.UpdatePreorderTraversal));
    }

    public void UpdatePostorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.Instance.SetTraversalMode(new TraversalDelegate(AlgorithmTreeManager.TreeInterface.UpdatePostorderTraversal));
    }

    public void UpdateLevelorderTraversal()
    {
        TraversalReset();
        AlgorithmTreeManager.Instance.SetTraversalMode(new TraversalDelegate(AlgorithmTreeManager.TreeInterface.UpdateLevelorderTraversal));
    }


    public static void InstantiateNodeInfo(int value){
        GameObject g = Instantiate(_staticNodeInfoPrefab, _staticNodeInfoParent);
        g.GetComponent<TMP_Text>().text =value.ToString();
    }

    private void TraversalReset(){
        for (int k = _nodeInfoParent.childCount - 1; k >= 0; k--) Destroy(_nodeInfoParent.GetChild(k).gameObject);
        AlgorithmTreeManager.BTree.ResetRecentNode();
    }


}
