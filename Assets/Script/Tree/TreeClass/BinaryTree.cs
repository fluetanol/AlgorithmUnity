using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BinaryTree{
    public Node Root;
    protected Node _recentFindNode = null;
    protected Color _originNodeColor;
    protected Queue<Node> queue = new();
    protected HashSet<int> treeValue = new();
    protected int _treeNodeCount = 0;
    protected int _height = 0;

    public abstract bool Add(Node node);
    public abstract (GameObject, GameObject) Remove(int Value);
    public abstract Node Find(int Value);
    public abstract bool isExist(int Value);


    public virtual void ResetRecentNode(){
        if (_recentFindNode != null) _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _originNodeColor);
        _recentFindNode = null;
    }

    public virtual void SetRootValue(int value){
        Root.Value = value;
        Root.NodeObject.GetComponent<NodeObjectInfo>().NodeValueText.text = value.ToString();
    }

    //새 노드 추가시 시각적 처리
    protected virtual void PlaceNodeObject(ref Node node, ref Node currentNode, bool isLeft, float depth) {
        _treeNodeCount += 1;
        node.Parent = currentNode;

        if (isLeft) currentNode.left = node;
        else currentNode.right = node;

        var ParentNodeInfo = node.Parent.NodeObject.GetComponent<NodeObjectInfo>();
        var ChildNodeInfo = node.NodeObject.GetComponent<NodeObjectInfo>();
        var ConnectInfo = node.ConnectObject.GetComponent<NodeConnectObejctInfo>();

        node.ConnectObject.transform.SetParent(node.Parent.NodeObject.transform);
        node.NodeObject.transform.SetParent(node.Parent.NodeObject.transform);

        if (isLeft) ConnectInfo.transform.eulerAngles = -1 * ConnectInfo.transform.eulerAngles;
        else ConnectInfo.transform.eulerAngles = ConnectInfo.transform.eulerAngles;


        Vector3 a = ConnectInfo.transform.position;
        Vector3 b = ConnectInfo.StartPoint.position;

        if (isLeft) ConnectInfo.transform.position = ParentNodeInfo.leftNodePoint.position + (a - b);
        else ConnectInfo.transform.position = ParentNodeInfo.rightNodePoint.position + (a - b);

        ChildNodeInfo.transform.position = ConnectInfo.EndPoint.transform.position;
        ChildNodeInfo.NodeValueText.text = node.Value.ToString();
    }



    public int GetNodeCount() => _treeNodeCount;
    public void PostOrderTraversal() => postOrder(Root);
    public void PreorderTraversal() => preOrder(Root);
    public void InorderTraversal() => inOrder(Root);
    public void LevelorderTraversal() => levelOrder(Root);

    //절차적인 inordertraversal
    public void UpdateInorderTraversal(ref Node node)
    {
        //탐색 끝
        if (checkUpdateTraversalFinish(false)) return;
        //좌노드 있는지 + 한번도 안 둘러본 노드가 맞는지 확인
        if (node.left != null && !treeValue.Contains(node.left.Value)) node = node.left;
        //좌노드는 있는데 이미 둘러본 노드인 경우 (다 둘러봐서 parent로 올라온 경우) 또는 좌노드가 없는 경우
        else if ((node.left != null && treeValue.Contains(node.left.Value)) || node.left == null)
        {
            if (!treeValue.Contains(node.Value)) UpdateTraversalNodeVisual(ref node);
            //오른쪽에 노드가 있는 케이스, 오른쪽으로 가면 됨.
            if (node.right != null && !treeValue.Contains(node.right.Value)) node = node.right;
            else node = node.Parent;
        }
    }

    /* 실험중
    TODO: 코루틴으로 inorder traversal 구현
    public void CoroutineInorderTraversal(){
        
    }


    private IEnumerator inorder(Node node){
        if (node == null) yield break;

        yield return inorder(node.left);

        TreeUIManager.InstantiateNodeInfo(node.Value);
        yield return null;

        yield return inorder(node.right);
    }*/



    //절차적인 preOrder
    public void UpdatePreorderTraversal(ref Node node)
    {
        //탐색 끝
        if (checkUpdateTraversalFinish(false)) return;
        if (node != null && !treeValue.Contains(node.Value)) UpdateTraversalNodeVisual(ref node);

        //좌노드 있는지 + 한번도 안 둘러본 노드가 맞는지 확인
        if (node.left != null && !treeValue.Contains(node.left.Value)) node = node.left;
        //좌노드는 있는데 이미 둘러본 노드인 경우 (다 둘러봐서 parent로 올라온 경우) 또는 좌노드가 없는 경우
        else if ((node.left != null && treeValue.Contains(node.left.Value)) || node.left == null)
        {
            //오른쪽에 노드가 있는 케이스, 오른쪽으로 가고 그 노드를 포함시킴
            if (node.right != null && !treeValue.Contains(node.right.Value)) node = node.right;
            else node = node.Parent;
        }
    }


    public void UpdatePostorderTraversal(ref Node node)
    {
        if (checkUpdateTraversalFinish(false)) return;

        //좌노드 있는지 + 한번도 안 둘러본 노드가 맞는지 확인
        if (node.left != null && !treeValue.Contains(node.left.Value)) node = node.left;

        //좌노드는 있는데 이미 둘러본 노드인 경우 (다 둘러봐서 parent로 올라온 경우) 또는 좌노드가 없는 경우
        else if ((node.left != null && treeValue.Contains(node.left.Value)) || node.left == null)
        {
            //오른쪽에 노드가 있는 케이스, 오른쪽으로 가고 그 노드를 포함시킴
            if (node.right != null && !treeValue.Contains(node.right.Value)) node = node.right;
            else
            {
                UpdateTraversalNodeVisual(ref node);
                node = node.Parent;
            }
        }
    }

    public void UpdateLevelorderTraversal(ref Node node)
    {
        UpdateTraversalNodeVisual(ref node);

        if (node.left != null) queue.Enqueue(node.left);
        if (node.right != null) queue.Enqueue(node.right);
        if (queue.Count == 0)
        {
            checkUpdateTraversalFinish(true);
            return;
        }
        node = queue.Dequeue();
    }

    private void inOrder(Node node)
    {
        if (node == null) return;
        inOrder(node.left);
        TreeUIManager.InstantiateNodeInfo(node.Value);
        inOrder(node.right);
    }

    private void preOrder(Node node)
    {
        if (node == null) return;
        TreeUIManager.InstantiateNodeInfo(node.Value);
        preOrder(node.left);
        preOrder(node.right);
    }

    private void postOrder(Node node)
    {
        if (node == null) return;
        postOrder(node.left);
        postOrder(node.right);
        TreeUIManager.InstantiateNodeInfo(node.Value);
    }

    private void levelOrder(Node node)
    {
        TreeUIManager.InstantiateNodeInfo(node.Value);
        if (node.left != null) queue.Enqueue(node.left);
        if (node.right != null) queue.Enqueue(node.right);
        if (queue.Count == 0) return;
        levelOrder(queue.Dequeue());
    }

    private void UpdateTraversalNodeVisual(ref Node node)
    {
        AlgorithmTreeManager.RollBackTime();
        TreeUIManager.InstantiateNodeInfo(node.Value);
        if (_recentFindNode != null) _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _originNodeColor);
        node.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.cyan);
        _recentFindNode = node;
        treeValue.Add(node.Value);
    }

    private bool checkUpdateTraversalFinish(bool isLevelOrder)
    {
        if (isLevelOrder || treeValue.Count == _treeNodeCount)
        {
            AlgorithmTreeManager.SetTraversalMode(null);
            AlgorithmTreeManager.RollBackStartNode();
            _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _originNodeColor);
            treeValue.Clear();
            return true;
        }
        else return false;
    }


}