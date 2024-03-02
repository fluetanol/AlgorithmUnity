using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class BinaryTree : TreeInterface{
    public Node Root;

    private Node _recentFindNode = null;
    private int _treeNodeCount = 0;
    private int _height = 0;

    public BinaryTree(){
        Root = new(0);
        _treeNodeCount += 1;
        _height += 1;
    }

    public bool Add(Node node) =>AddNode(node, Root, 1);
    public Node Find(int Value) => findNode(Value, Root);
    public int GetNodeCount() => _treeNodeCount;
    public bool isExist() => false;


    private bool AddNode(Node node, Node currentNode, int depth){
        bool isFind = true;
        if(node.Value < currentNode.Value){
            if(currentNode.left == null)  PlaceNodeObject(ref node, ref currentNode, true, depth);
            else{
                currentNode = currentNode.left;
                depth+=1;
                isFind = AddNode(node, currentNode, depth);
            }
        }
        else if(node.Value > currentNode.Value){
            if (currentNode.right == null)  PlaceNodeObject(ref node, ref currentNode, false, depth);
            else{
                currentNode = currentNode.right;
                depth+=1;
                isFind = AddNode(node, currentNode, depth);
            }
        }
        else if (node.Value == currentNode.Value) return false;
        return isFind;
    }


    private Node findNode(int Value, Node node){
        Node find = null;

        if(node==null)  return null;
        if(Value == node.Value) {
            if(_recentFindNode!=null) _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", node.NodeObject.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));
            node.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.yellow);
            _recentFindNode = node;
            return node;
        }
        if(Value < node.Value) find = findNode(Value, node.left);
        else if(Value > node.Value) find = findNode(Value, node.right);
        return find;
    }


    public void BFS()
    {
        throw new System.NotImplementedException();
    }

    public void DFS()
    {
        throw new System.NotImplementedException();
    }

    public void InorderTraversal(){
        throw new System.NotImplementedException();
    }

    private void PlaceNodeObject(ref Node node, ref Node currentNode, bool isLeft, float depth)
    {
        _treeNodeCount +=1;
        node.Parent = currentNode;

        if (isLeft) currentNode.left = node;
        else currentNode.right = node;

        var ParentNodeInfo = node.Parent.NodeObject.GetComponent<NodeObjectInfo>();
        var ChildNodeInfo = node.NodeObject.GetComponent<NodeObjectInfo>();
        var ConnectInfo = node.ConnectObject.GetComponent<NodeConnectObejctInfo>();
        
        Debug.Log(depth);
        if (isLeft) ConnectInfo.transform.eulerAngles = ConnectInfo.transform.eulerAngles * (1f/depth);
        else ConnectInfo.transform.eulerAngles = -1 * ConnectInfo.transform.eulerAngles * (1f/depth);

        node.ConnectObject.transform.SetParent(node.Parent.NodeObject.transform);
        node.NodeObject.transform.SetParent(node.Parent.NodeObject.transform);

        if (isLeft) ConnectInfo.transform.position = ParentNodeInfo.leftNodePoint.position;
        else ConnectInfo.transform.position = ParentNodeInfo.rightNodePoint.position;

        ChildNodeInfo.transform.position = ConnectInfo.EndPoint.transform.position;
        ChildNodeInfo.NodeValueText.text = node.Value.ToString();
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