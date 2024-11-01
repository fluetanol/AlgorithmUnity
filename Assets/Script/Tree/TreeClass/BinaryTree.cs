
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
        if (_recentFindNode != null) 
        _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _originNodeColor);
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


    public IEnumerator CoroutineInorderTraversal(Node node, float seconds)
    {
        if (node == null){
            yield break;
        }

        IEnumerator leftenumerator = CoroutineInorderTraversal(node.left, seconds);
        while(leftenumerator.MoveNext()){
            yield return new WaitForSeconds(seconds);
        }

        UpdateTraversalNodeVisual(ref node);
        yield return new WaitForSeconds(seconds);

        IEnumerator rightenumerator = CoroutineInorderTraversal(node.right, seconds);
        while(rightenumerator.MoveNext())
            yield return new WaitForSeconds(seconds);

    }


    public IEnumerator CoroutinePostorderTraversal(Node node, float seconds)
    {
        if (node == null){
            yield break;
        }

        IEnumerator leftenumerator = CoroutinePostorderTraversal(node.left, seconds);
        while(leftenumerator.MoveNext()){
            yield return new WaitForSeconds(seconds);
        }
        

        IEnumerator rightenumerator = CoroutinePostorderTraversal(node.right, seconds); 
        while(rightenumerator.MoveNext()){
            yield return new WaitForSeconds(seconds);
        }
           

        UpdateTraversalNodeVisual(ref node);
        yield return new WaitForSeconds(seconds);
    }

    public IEnumerator CoroutinePreorderTraversal(Node node, float seconds)
    {
        if (node == null){
            yield break;
        }

        UpdateTraversalNodeVisual(ref node);
        yield return new WaitForSeconds(seconds);

        IEnumerator leftenumerator = CoroutinePreorderTraversal(node.left, seconds);
        while(leftenumerator.MoveNext()){
            yield return new WaitForSeconds(seconds);
        }

        IEnumerator rightenumerator = CoroutinePreorderTraversal(node.right, seconds);
        while(rightenumerator.MoveNext()){
            yield return new WaitForSeconds(seconds);
        }

    }
    /*
    private void levelOrder(Node node)
    {
        TreeUIManager.InstantiateNodeInfo(node.Value);
        if (node.left != null) queue.Enqueue(node.left);
        if (node.right != null) queue.Enqueue(node.right);
        if (queue.Count == 0) return;
        levelOrder(queue.Dequeue());
    }*/

    public IEnumerator CoroutineLevelorderTraversal(Node node, float seconds)
    {
        UpdateTraversalNodeVisual(ref node);
        yield return new WaitForSeconds(seconds);
        if (node.left != null) queue.Enqueue(node.left);
        if (node.right != null) queue.Enqueue(node.right);
        if (queue.Count == 0) yield break;

        IEnumerator enumerator = CoroutineLevelorderTraversal(queue.Dequeue(), seconds);
        while(enumerator.MoveNext())
            yield return new WaitForSeconds(seconds);
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
        TreeUIManager.InstantiateNodeInfo(node.Value);
        if (_recentFindNode != null) _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _originNodeColor);
        node.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.cyan);
        _recentFindNode = node;
        treeValue.Add(node.Value);
    }

    public void LevelOrderQueueReset(){
        queue.Clear();
    }

/*
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
*/

}