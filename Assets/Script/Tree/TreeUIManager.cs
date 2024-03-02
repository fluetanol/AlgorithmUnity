using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Profiling;
using UnityEngine;

public class TreeUIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text textField;

    public void AddNode(){
        if(int.TryParse(inputField.text, out int value)){
            GameObject NodeObject = Instantiate(AlgorithmTreeManager.Instance.NodePrefab);
            GameObject ConnectObject = Instantiate(AlgorithmTreeManager.Instance.ConnectPrefab);
            Node node = new Node(value);
            node.NodeObject = NodeObject;
            node.ConnectObject = ConnectObject;
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
            Node node = AlgorithmTreeManager.BinaryTree.Find(value);
            if(node == null)   textField.text = "NotFound";
            else textField.text = "Find!";
            
        }
        else textField.text = "Wrong Num";
    }

    public void InorderTraversal(){
        AlgorithmTreeManager.BinaryTree.InorderTraversal();
    }

}
