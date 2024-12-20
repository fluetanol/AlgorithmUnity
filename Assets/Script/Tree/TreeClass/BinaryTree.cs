
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public abstract class BinaryTree{
    public Node Root;
    protected Node _recentFindNode = null;
    protected Color _originNodeColor;
    protected Queue<Node> queue = new();
    protected HashSet<Node> treeNodeSet = new();
    protected int _treeNodeCount = 0;
    protected int _height = 0;

    public abstract bool Add(Node node, Edge edge);

    /// <summary>
    /// 찾은 값에 기반하여 삭제될 노드와 엣지 쌍을 반환
    /// </summary>
    /// <param name="Value"> 삭제하려는 값 </param>
    /// <returns>(삭제된 노드, 삭제된 엣지) </returns>
    public abstract GameObject Remove(int Value);


    public abstract Node Find(int Value);
    public abstract bool isExist(int Value, out Node node);

    private List<(Node, Vector3)> _nodePosList = new();   

    public virtual void ResetRecentNode(){
        if (_recentFindNode != null) 
        _recentFindNode.image.color = _originNodeColor;
        _recentFindNode = null;
    }

    public virtual void SetRootValue(int value){
        Root.Value = value;
        Root.SetNodeValue(value);
    }

    //새 노드 추가시 시각적 처리            //ParentNode : 추가될 노드의 부모 노드
    //currentNode : 추가될 노드의 부모 노드
    //Node : 추가 되는 노드
    protected virtual void PlaceNodeObject(ref Node node, ref Node parentNode, bool isLeft, float depth) {
        _treeNodeCount += 1;
        nodeConnect(ref parentNode, ref node, isLeft);
        node.isLeft = isLeft;
        nodeWidthControl(ref node, isLeft);

        node.transform.position = new Vector3
        (parentNode.transform.position.x + (isLeft ? -1 : 1),
        parentNode.transform.position.y - 1f, 0.5f);

        node.SetNodeValue(node.Value);
    }

    //노드 이동 관련 애니메이션으로, DOTween과 TreeUIManager등의 static함수들과의 의존성이 상당히 높은 구역입니다.
    //이거 바깥으로 빼는 방법이 없을까...?
    public void NodeMoveAnimation(Node focusNode, float seconds){
        TreeUIManager.current.CloseNodeInfoUI(0.5f);
        Camera.main.DOOrthoSize(4, seconds);
        Sequence sequence = DOTween.Sequence();

        foreach(var (node, dir) in _nodePosList){
            Vector3 targetPos = node.transform.position - dir;
            node.PositionMove(ref sequence, targetPos, seconds);
        }

        Vector3 focusNodePos = focusNode.transform.position;
        focusNodePos.z = Camera.main.transform.position.z;
        Camera.main.DOCameraMove(focusNodePos, seconds);

        _nodePosList.Clear();
    }


    private void nodeConnect(ref Node parentNode, ref Node node, bool isLeft){
        node.Parent = parentNode;
        if (isLeft) parentNode.left = node;
        else parentNode.right = node;
        node.transform.parent = parentNode.transform;
    }


    protected void nodeWidthControl(ref Node node, bool isLeft, bool isAdd = true){
        if (node == Root){
            return;
        }
        //add동작은 넓어져야 한다.
        else if (node.isLeft == isLeft){
            nodeWidthControl(ref node.Parent, isLeft, isAdd);
        }
        else if(node.isLeft != isLeft){
            float offset = isLeft ? -1 : 1;
            Vector3 dir = isAdd ? Vector3.right * offset : Vector3.left * offset;
            _nodePosList.Add((node,dir));
            nodeWidthControl(ref node.Parent, node.isLeft, isAdd);
        }
    }

    public int GetNodeCount() => _treeNodeCount;
    public void PostOrderTraversal() => postOrder(Root);
    public void PreorderTraversal() => preOrder(Root);
    public void InorderTraversal() => inOrder(Root);
    public void LevelorderTraversal() => levelOrder(Root);


    public IEnumerator CoroutineInorderTraversal(Node node, float seconds){
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
        if (_recentFindNode != null) _recentFindNode.image.color = _originNodeColor;
        node.image.color = Color.cyan;
        _recentFindNode = node;
    }

    public void LevelOrderQueueReset(){
        queue.Clear();
    }

}