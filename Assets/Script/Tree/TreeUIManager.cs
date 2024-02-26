using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeUIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void AddNode(){
        
        if(int.TryParse(inputField.text, out int value)){
            GameObject NodeObject = Instantiate(AlgorithmTreeManager.Instance.NodePrefab);
            GameObject ConnectObject = Instantiate(AlgorithmTreeManager.Instance.ConnectPrefab);
            Node node = new Node(value);
            node.NodeObject = NodeObject;
            node.ConnectObject = ConnectObject;
            if(AlgorithmTreeManager.BinaryTree.AddNode(node, AlgorithmTreeManager.BinaryTree.Root)){
                Debug.Log("Success Add Node");
            }
            else{
                Debug.Log("Fail Add Node");
                Destroy(NodeObject);
                Destroy(ConnectObject);
            }

        }
        else{
            Debug.Log("Wrong Num");
        }
    }
}
