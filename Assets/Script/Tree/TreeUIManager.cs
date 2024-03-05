using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Profiling;
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

    public void AddNode(){
        if(int.TryParse(inputField.text, out int value)){
            GameObject NodeObject = Instantiate(AlgorithmTreeManager.Instance.NodePrefab);
            GameObject ConnectObject = Instantiate(AlgorithmTreeManager.Instance.ConnectPrefab);
            Node node = new Node(){
                Value = value,
                NodeObject = NodeObject,
                ConnectObject = ConnectObject
            };
            if(AlgorithmTreeManager.BinaryTree.Add(node)){
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
            if(AlgorithmTreeManager.BinaryTree.isExist(value)){
                textField.text = "Find!";
            }
            else textField.text = "NotFound";
        }
        else textField.text = "Wrong Num";
    }

    public void RemoveNode(){
        if (int.TryParse(inputField.text, out int value))
        {
            (GameObject, GameObject) RemoveObject = AlgorithmTreeManager.BinaryTree.Remove(value);
            if (RemoveObject.Item1 == null) textField.text = "NotFound";
            else {
                Destroy(RemoveObject.Item1);
                Destroy(RemoveObject.Item2);
                textField.text = "Remove!";
            }
        }
    }

    public void SetRootNodeValue(){
        if (int.TryParse(inputField.text, out int value) && AlgorithmTreeManager.BinaryTree.GetNodeCount() == 1){
            AlgorithmTreeManager.BinaryTree.SetRootValue(value);
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
        AlgorithmTreeManager.BinaryTree.PreorderTraversal();
    }

    public void InorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.BinaryTree.InorderTraversal();
    }

    public void PostorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.BinaryTree.PostOrderTraversal();
    }

    public void LevelorderTraversal()
    {
        TraversalReset();
        AlgorithmTreeManager.BinaryTree.LevelorderTraversal();
    }

    public void UpdateInorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.Instance.SetTraversalMode(new AlgorithmTreeManager.TraversalDelegate(AlgorithmTreeManager.BinaryTree.UpdateInorderTraversal));
    }

    public void UpdatePreorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.Instance.SetTraversalMode(new AlgorithmTreeManager.TraversalDelegate(AlgorithmTreeManager.BinaryTree.UpdatePreorderTraversal));
    }

    public void UpdatePostorderTraversal(){
        TraversalReset();
        AlgorithmTreeManager.Instance.SetTraversalMode(new AlgorithmTreeManager.TraversalDelegate(AlgorithmTreeManager.BinaryTree.UpdatePostorderTraversal));
    }

    public void UpdateLevelorderTraversal()
    {
        TraversalReset();
        AlgorithmTreeManager.Instance.SetTraversalMode(new AlgorithmTreeManager.TraversalDelegate(AlgorithmTreeManager.BinaryTree.UpdateLevelorderTraversal));
    }


    public static void InstantiateNodeInfo(int value){
        GameObject g = Instantiate(_staticNodeInfoPrefab, _staticNodeInfoParent);
        g.GetComponent<TMP_Text>().text =value.ToString();
    }

    private void TraversalReset(){
        for (int k = _nodeInfoParent.childCount - 1; k >= 0; k--) Destroy(_nodeInfoParent.GetChild(k).gameObject);
        AlgorithmTreeManager.BinaryTree.ResetRecentNode();
    }


}
