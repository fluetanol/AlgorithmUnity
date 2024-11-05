
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

    public abstract bool Add(Node node, Edge edge);
    public abstract (GameObject, GameObject) Remove(int Value);
    public abstract Node Find(int Value);
    public abstract bool isExist(int Value);

    public virtual void ResetRecentNode(){
        if (_recentFindNode != null) 
        _recentFindNode.GetComponent<SpriteRenderer>().color = _originNodeColor;
        _recentFindNode = null;
    }

    public virtual void SetRootValue(int value){
        Root.Value = value;
        Root.SetNodeValueText(value);
    }

    //새 노드 추가시 시각적 처리            //ParentNode : 추가될 노드의 부모 노드
    //currentNode : 추가될 노드의 부모 노드
    //Node : 추가 되는 노드
    protected virtual void PlaceNodeObject(ref Node node, ref Node parentNode, bool isLeft, float depth) {
        _treeNodeCount += 1;

        nodeConnect(ref parentNode, ref node, isLeft);
        node.isLeft = isLeft;
        var ParentNodeInfo = node.Parent;
        var NodeInfo = node;

        if(node.Parent.isLeft && node.Parent != Root){
            node.Parent.SetPositionOffset(-1f);
            //node.Parent.SetCenterPos();
        }
        else if(!node.Parent.isLeft && node.Parent != Root){
            node.Parent.SetPositionOffset(1f);
          // node.Parent.SetCenterPos();
        }
        NodeInfo.transform.position = new Vector3
        (ParentNodeInfo.transform.position.x + (isLeft ? -1 : 1),
        ParentNodeInfo.transform.position.y - 1f, 0);
        
        NodeInfo.SetDepthText((int)depth);
        NodeInfo.SetNodeValueText(node.Value);
    }


    private void nodeConnect(ref Node parentNode, ref Node node, bool isLeft){
        node.Parent = parentNode;
        if (isLeft) parentNode.left = node;
        else parentNode.right = node;

        node.transform.parent = parentNode.transform;
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
        if (_recentFindNode != null) _recentFindNode.GetComponent<SpriteRenderer>().color = _originNodeColor;
        node.GetComponent<SpriteRenderer>().color = Color.cyan;
        _recentFindNode = node;
        treeValue.Add(node.Value);
    }

    public void LevelOrderQueueReset(){
        queue.Clear();
    }



}