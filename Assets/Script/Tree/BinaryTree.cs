using System.Collections;

using UnityEngine;

public class BinaryTree : TreeInterface{
    public Node Root;
    private int treeNodeCount;

    public BinaryTree(){
        Root = new(0);

    }

    public bool AddNode(Node node, Node currentNode){
        if(node.Value < currentNode.Value){
            if(currentNode.left == null){
                node.Parent = currentNode;
                currentNode.left = node;

                var ParentNodeInfo = node.Parent.NodeObject.GetComponent<NodeObjectInfo>();
                var ChildNodeInfo = node.NodeObject.GetComponent<NodeObjectInfo>();
                var ConnectInfo = node.ConnectObject.GetComponent<NodeConnectObejctInfo>();
                
                node.ConnectObject.transform.SetParent(node.Parent.NodeObject.transform);
                node.NodeObject.transform.SetParent(node.Parent.NodeObject.transform);

                ConnectInfo.transform.position =  ParentNodeInfo.leftNodePoint.position;
                ChildNodeInfo.transform.position = ConnectInfo.EndPoint.transform.position;
                ChildNodeInfo.NodeValueText.text = node.Value.ToString();
                return true;
            }
            currentNode = currentNode.left;
            AddNode(node, currentNode);
        }
        else if(node.Value > currentNode.Value){
            if (currentNode.right == null){
                node.Parent = currentNode;
                currentNode.right = node;

                var ParentNodeInfo = node.Parent.NodeObject.GetComponent<NodeObjectInfo>();
                var ChildNodeInfo = node.NodeObject.GetComponent<NodeObjectInfo>();
                var ConnectInfo = node.ConnectObject.GetComponent<NodeConnectObejctInfo>();
                ConnectInfo.transform.eulerAngles = -1 * ConnectInfo.transform.eulerAngles;

                node.ConnectObject.transform.SetParent(node.Parent.NodeObject.transform);
                node.NodeObject.transform.SetParent(node.Parent.NodeObject.transform);

                ConnectInfo.transform.position = ParentNodeInfo.rightNodePoint.position;
                ChildNodeInfo.transform.position = ConnectInfo.EndPoint.transform.position;
                ChildNodeInfo.NodeValueText.text = node.Value.ToString();

                return true;
            }
            currentNode = currentNode.right;
            AddNode(node, currentNode);
        }
        else return false;

        return true;
    }

    public void SetParent(Node child, Node parent){
        child.Parent = parent;
    }

    public void SetRight(Node node, Node right){
        node.right = right;
    }

    public void SetLeft(Node node, Node left){
        node.left = left;
    }

    public int GetNodeCount() => treeNodeCount;

    public void BFS()
    {
        throw new System.NotImplementedException();
    }

    public void DFS()
    {
        throw new System.NotImplementedException();
    }
}


public class Node{
    public Node right = null;
    public Node left =  null;
    public Node Parent = null;
    public int Value;
    public GameObject NodeObject;
    public GameObject ConnectObject;

    public Node(int value){
        Value = value;
    }


}