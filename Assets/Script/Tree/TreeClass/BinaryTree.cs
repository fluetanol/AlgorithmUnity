using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Node
{
    public Node right = null;
    public Node left = null;
    public Node Parent = null;
    public int Value;
    public GameObject NodeObject;
    public GameObject ConnectObject;

}

public class BinaryTree : TreeInterface{
    public Node Root;

    private Node _recentFindNode = null;
    private int _treeNodeCount = 0;
    private int _height = 0;

    private HashSet<int> treeValue =  new();
    private Queue<Node> queue = new();
    private Color _originNodeColor;

    public BinaryTree(GameObject rootObject, int rootValue){
        Root = new(){
            Value = rootValue,
            NodeObject = rootObject,
        };
        _originNodeColor = Root.NodeObject.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        _treeNodeCount += 1;
        _height += 1;
    }

    public bool Add(Node node) =>addNode(node, Root, 1);
    public Node Find(int Value) => findNode(Value, Root);
    public (GameObject,GameObject) Remove(int Value) => removeNode(Value, Root);
    public void SetRootValue(int value) => Root.Value = value;
    public int GetNodeCount() => _treeNodeCount;
    public bool isExist() => false;


    private bool addNode(Node node, Node currentNode, int depth){
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
                isFind = addNode(node, currentNode, depth);
            }
        }
        else if(node.Value > currentNode.Value){
            if (currentNode.right == null)  PlaceNodeObject(ref node, ref currentNode, false, depth);
            else{
                currentNode = currentNode.right;
                depth+=1;
                isFind = addNode(node, currentNode, depth);
            }
        }
        else if (node.Value == currentNode.Value) return false;
        return isFind;
    }

    private Node findNode(int Value, Node node){
        Node find = null;
        if(node==null)  return null;
        if(Value == node.Value) {
            if(_recentFindNode!=null) _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _originNodeColor);
            node.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.yellow);
            _recentFindNode = node;
            return node;
        }
        if(Value < node.Value) find = findNode(Value, node.left);
        else if(Value > node.Value) find = findNode(Value, node.right);
        return find;
    }


    private Node findNode(int Value, Node node, ref bool isLeft)
    {
        Node find = null;
        if (node == null) return null;
        if (Value == node.Value) return node;
        if (Value < node.Value) {
            isLeft = true;
            find = findNode(Value, node.left,ref isLeft);  
        }
        else if (Value > node.Value) {
            isLeft = false;
            find = findNode(Value, node.right, ref isLeft);
        }
        return find;
    }


    private (GameObject, GameObject) removeNode(int Value, Node node){
        bool isLeft = false;
        Node findnode = findNode(Value, node, ref isLeft);

        if(findnode == Root)  return (Root.NodeObject, null);
        if (findnode == null) return (null, null);
        
        GameObject removeObject = null;
        GameObject removeConnectObject = null;

        int count = 0;
        if(findnode.left !=null) count+=1;
        if(findnode.right!=null) count+=1;


        removeObject = findnode.NodeObject;
        removeConnectObject = findnode.ConnectObject;
        if (isLeft) findnode.Parent.left = null;
        else findnode.Parent.right = null;
        
        switch (count){
            case 1:
                if(findnode.left == null)  OneChildNodeRmoveUpdate(ref findnode.Parent, ref findnode.right, ref removeObject,ref removeConnectObject, isLeft);
                else if(findnode.right == null)  OneChildNodeRmoveUpdate(ref findnode.Parent, ref findnode.left, ref removeObject, ref removeConnectObject, isLeft);
            break;

            case 2:
                Node smallestNode = findSmallestNode(findnode.right);
                smallestNode.Parent = findnode.Parent;
                break;
        }
        _treeNodeCount-=1;
        return (removeObject, removeConnectObject);
    }


    public void PostOrderTraversal() =>  postorder(Root);
    public void PreorderTraversal() =>   preorder(Root);
    public void InorderTraversal() =>  inorder(Root);
    public void LevelorderTraversal() => levelorder(Root);


    //절차적인 inordertraversal
    public void UpdateInorderTraversal(ref Node node){
        //탐색 끝
        if (checkUpdateTraversalFinish(false)) return;
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
        if(checkUpdateTraversalFinish(false)) return;
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
        if (checkUpdateTraversalFinish(false)) return;

        //좌노드 있는지 + 한번도 안 둘러본 노드가 맞는지 확인
        if (node.left != null && !treeValue.Contains(node.left.Value)) {
            node = node.left;
        }
        //좌노드는 있는데 이미 둘러본 노드인 경우 (다 둘러봐서 parent로 올라온 경우) 또는 좌노드가 없는 경우
        else if ((node.left != null && treeValue.Contains(node.left.Value)) || node.left == null)
        {
            //오른쪽에 노드가 있는 케이스, 오른쪽으로 가고 그 노드를 포함시킴
            if (node.right != null && !treeValue.Contains(node.right.Value)) {
                node = node.right;
            }
            else {
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
        if (queue.Count == 0){
            checkUpdateTraversalFinish(true);
            return;
        }
        node = queue.Dequeue();
    }


    public void ResetRecentNode() => _recentFindNode = null;

    private Node findSmallestNode(Node node){
        if(node.left == null) return node;
        return findSmallestNode(node.left);
    }


    private void OneChildNodeRmoveUpdate(ref Node parent,ref Node child, ref GameObject removeObject,ref  GameObject removeConnectObject, bool isLeft){
        if(isLeft) parent.left = child;
        else parent.right = child;

        child.Parent = parent;
        child.NodeObject.transform.position = removeObject.transform.position;
        child.NodeObject.transform.parent = parent.NodeObject.transform;
        child.ConnectObject.transform.parent = parent.NodeObject.transform;
        child.ConnectObject.transform.position = removeConnectObject.transform.position;
        child.ConnectObject.transform.rotation = removeConnectObject.transform.rotation;
    }

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

    private void postorder(Node node){
        if (node == null) return;
        postorder(node.left);
        postorder(node.right);
        TreeUIManager.InstantiateNodeInfo(node.Value);
    }

    private void levelorder(Node node){
        TreeUIManager.InstantiateNodeInfo(node.Value);
        if (node.left!=null) queue.Enqueue(node.left);
        if(node.right!=null) queue.Enqueue(node.right);
        if(queue.Count == 0) return;
        levelorder(queue.Dequeue());
    }




    //새 노드 추가시 시각적 처리
    private void PlaceNodeObject(ref Node node, ref Node currentNode, bool isLeft, float depth){
        _treeNodeCount +=1;
        Debug.Log("treeCount: "+_treeNodeCount + " depth : " +depth);
        node.Parent = currentNode;

        if (isLeft) currentNode.left = node;
        else currentNode.right = node;

        var ParentNodeInfo = node.Parent.NodeObject.GetComponent<NodeObjectInfo>();
        var ChildNodeInfo = node.NodeObject.GetComponent<NodeObjectInfo>();
        var ConnectInfo = node.ConnectObject.GetComponent<NodeConnectObejctInfo>();


        node.ConnectObject.transform.SetParent(node.Parent.NodeObject.transform);
        node.NodeObject.transform.SetParent(node.Parent.NodeObject.transform);
        //node.NodeObject.transform.localScale /= (float)Math.Pow(2 , depth);

        //float angle = 0;
        //for(int i=1; i<depth; i++) angle -= 45f/(float)Math.Pow(2, i);
        //ConnectInfo.transform.Rotate(0,0, angle);

        if (isLeft) ConnectInfo.transform.eulerAngles = -1* ConnectInfo.transform.eulerAngles;
        else ConnectInfo.transform.eulerAngles = ConnectInfo.transform.eulerAngles;
        

        Vector3 a = ConnectInfo.transform.position;
        Vector3 b = ConnectInfo.StartPoint.position;

        if (isLeft) ConnectInfo.transform.position = ParentNodeInfo.leftNodePoint.position + (a - b);
        else ConnectInfo.transform.position = ParentNodeInfo.rightNodePoint.position + (a - b);


        ChildNodeInfo.transform.position = ConnectInfo.EndPoint.transform.position;
        ChildNodeInfo.NodeValueText.text = node.Value.ToString();
    }


    //시각적인 표현만 해주는 거임
    private void UpdateTraversalNodeVisual(ref Node node){
        AlgorithmTreeManager.Instance.RollBackTime();
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
            AlgorithmTreeManager.Instance.SetTraversalMode(AlgorithmTreeManager.Traversalmode.None);
            AlgorithmTreeManager.Instance.RollBackStartNode();
            _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _originNodeColor);
            treeValue.Clear();
            return true;
        }
        else return false;
    }
}