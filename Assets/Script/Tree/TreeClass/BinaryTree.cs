using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;


public class Node
{
    public Node right = null;
    public Node left = null;
    public Node Parent = null;
    public int Value;
    public GameObject NodeObject;
    public GameObject ConnectObject;

    public Node(int value)
    {
        Value = value;
    }
}

public class BinaryTree : TreeInterface{
    public Node Root;

    private Node _recentFindNode = null;
    private int _treeNodeCount = 0;
    private int _height = 0;

    private HashSet<int> treeValue =  new();

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
        if(depth>_height) {
            _height = depth;
            Camera.main.orthographicSize += 0.5f;
        }

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


    public void PostOrderTraversal(){
        postorder(Root);
    }

    public void PreorderTraversal(){
        preorder(Root);
    }

    //빠른 inordertraversal
    public void InorderTraversal(){
        inorder(Root);
    }

    //절차적인 inordertraversal
    public void UpdateInorderTraversal(ref Node node){
        //탐색 끝
        if (checkUpdateTraversalFinish()) return;
        //좌노드 있는지 + 한번도 안 둘러본 노드가 맞는지 확인
        if (node.left!= null && !treeValue.Contains(node.left.Value)) node= node.left;
        //좌노드는 있는데 이미 둘러본 노드인 경우 (다 둘러봐서 parent로 올라온 경우) 또는 좌노드가 없는 경우
        else if((node.left != null && treeValue.Contains(node.left.Value)) || node.left == null)
        {
            if (!treeValue.Contains(node.Value)) UpdateTraversalNodeVisual(ref node);
            //오른쪽에 노드가 있는 케이스, 오른쪽으로 가면 됨.
            if (node.right != null && !treeValue.Contains(node.right.Value)) node = node.right;
            else node = node.Parent;
        }
    }

    //절차적인 preorder
    public void UpdatePreorderTraversal(ref Node node)
    {
        //탐색 끝
        if(checkUpdateTraversalFinish()) return;
        if(node !=null && !treeValue.Contains(node.Value)) UpdateTraversalNodeVisual(ref node);

        //좌노드 있는지 + 한번도 안 둘러본 노드가 맞는지 확인
        if (node.left != null && !treeValue.Contains(node.left.Value))  node = node.left;
        //좌노드는 있는데 이미 둘러본 노드인 경우 (다 둘러봐서 parent로 올라온 경우) 또는 좌노드가 없는 경우
        else if ((node.left != null && treeValue.Contains(node.left.Value)) || node.left == null)
        {
            //오른쪽에 노드가 있는 케이스, 오른쪽으로 가고 그 노드를 포함시킴
            if (node.right != null && !treeValue.Contains(node.right.Value))   node = node.right;
            else node = node.Parent;
        }
    }


    public void UpdatePostorderTraversal(ref Node node)
    {
        if (checkUpdateTraversalFinish()) return;

        //좌노드 있는지 + 한번도 안 둘러본 노드가 맞는지 확인
        if (node.left != null && !treeValue.Contains(node.left.Value)) {
            node = node.left;
        }
        //좌노드는 있는데 이미 둘러본 노드인 경우 (다 둘러봐서 parent로 올라온 경우) 또는 좌노드가 없는 경우
        else if ((node.left != null && treeValue.Contains(node.left.Value)) || node.left == null)
        {
            UpdateTraversalNodeVisual(ref node);
            //오른쪽에 노드가 있는 케이스, 오른쪽으로 가고 그 노드를 포함시킴
            if (node.right != null && !treeValue.Contains(node.right.Value)) {
                node = node.right;
            }
            else node = node.Parent;   
        }
    }

    public void ResetRecentNode() => _recentFindNode = null;

    private void inorder(Node node){
        if(node==null) return;
        inorder(node.left);
        TreeUIManager.InstantiateNodeInfo(node.Value);
        inorder(node.right);
    }

    private void preorder(Node node){
        if(node==null)   return;
        TreeUIManager.InstantiateNodeInfo(node.Value);
        preorder(node.left);
        preorder(node.right);
    }

    private void postorder(Node node)
    {
        if (node == null) return;
        postorder(node.left);
        postorder(node.right);
        TreeUIManager.InstantiateNodeInfo(node.Value);
    }


    private void PlaceNodeObject(ref Node node, ref Node currentNode, bool isLeft, float depth)
    {
        _treeNodeCount +=1;
        //Debug.Log("treeCount: "+_treeNodeCount);
        node.Parent = currentNode;

        if (isLeft) currentNode.left = node;
        else currentNode.right = node;

        var ParentNodeInfo = node.Parent.NodeObject.GetComponent<NodeObjectInfo>();
        var ChildNodeInfo = node.NodeObject.GetComponent<NodeObjectInfo>();
        var ConnectInfo = node.ConnectObject.GetComponent<NodeConnectObejctInfo>();
    
        if (isLeft) ConnectInfo.transform.eulerAngles = ConnectInfo.transform.eulerAngles;
        else ConnectInfo.transform.eulerAngles = -1 * ConnectInfo.transform.eulerAngles ;

        node.ConnectObject.transform.SetParent(node.Parent.NodeObject.transform);
        node.NodeObject.transform.SetParent(node.Parent.NodeObject.transform);

        if (isLeft) ConnectInfo.transform.position = ParentNodeInfo.leftNodePoint.position;
        else ConnectInfo.transform.position = ParentNodeInfo.rightNodePoint.position;

        ChildNodeInfo.transform.position = ConnectInfo.EndPoint.transform.position;
        ChildNodeInfo.NodeValueText.text = node.Value.ToString();
    }

    private void UpdateTraversalNodeVisual(ref Node node){
        AlgorithmTreeManager.Instance.RollBackTime();
        TreeUIManager.InstantiateNodeInfo(node.Value);
        //시각적인 표현만 해주는 거임
        if (_recentFindNode != null) _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", node.NodeObject.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));
        node.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.cyan);
        _recentFindNode = node;
        treeValue.Add(node.Value);
    }


    private bool checkUpdateTraversalFinish()
    {
        if (treeValue.Count == _treeNodeCount)
        {
            AlgorithmTreeManager.Instance.SetTraversalMode(AlgorithmTreeManager.Traversalmode.None);
            AlgorithmTreeManager.Instance.RollBackStartNode();
            _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green);
            treeValue.Clear();
            return true;
        }
        else
        {
            return false;
        }
    }

}