using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public sealed class BinarySearchTree : BinaryTree{
    public BinarySearchTree(ref Node rootNode, int rootValue){
        Root = rootNode;
        rootNode.Value = rootValue;
        SetRootValue(rootValue);
        _originNodeColor = Root.GetComponentInChildren<Image>().color;
        _treeNodeCount += 1;
        _height += 1;
    }

    public override bool Add(Node node, Edge edge) =>addNode(node, edge, Root, 1);
    public override GameObject Remove(int Value) => removeNode(Value, Root);
    public override Node Find(int Value) => findNode(Value, Root);

    public override bool isExist(int Value, out Node node){
        Node findNode = Find(Value);
        if (findNode != null)
        {
            showFindNode(ref findNode);
            node = findNode;
            return true;
        }
        else {
            node = null;
            return false;
        }
    }

    private Dictionary<(int, int), Edge> _nodeSet = new();

    private bool addNode(Node node, Edge edge, Node currentNode, int depth){
        bool isFind = true;
        if(depth>_height) {
            _height = depth;
        }
        if(node.Value < currentNode.Value){
            if(currentNode.left == null)  {
                treeNodeSet.Add(node);
                PlaceNodeObject(ref node, ref currentNode, true, depth);
                edge.SetEdgeNode(node, currentNode);
            }
            else{
                currentNode = currentNode.left;
                depth+=1;
                isFind = addNode(node, edge, currentNode, depth);
            }
        }
        else if(node.Value > currentNode.Value){
            if (currentNode.right == null)  {
                treeNodeSet.Add(node);
                PlaceNodeObject(ref node, ref currentNode, false, depth);
                edge.SetEdgeNode(node, currentNode);
            }
            else{
                currentNode = currentNode.right;
                depth+=1;
                isFind = addNode(node, edge, currentNode, depth);
            }
        }
        else if (node.Value == currentNode.Value) return false;
        return isFind;
    }


    private Node findNode(int Value, Node node){
        Node find = null;
        if(node==null)  return null;
        if(Value == node.Value)  return node;
        if(Value < node.Value) find = findNode(Value, node.left);
        else if(Value > node.Value) find = findNode(Value, node.right);

        return find;
    }

    private void showFindNode(ref Node node){
        if (_recentFindNode != null) {
            _recentFindNode.image.color = _originNodeColor;
            //EventSystem.current.SetSelectedGameObject(null);
        }
        node.image.color = node.button.colors.selectedColor;
        _recentFindNode = node;
    }

    private GameObject removeNode(int value, Node node){
        ResetRecentNode();
        Node findnode = findNode(value, node);
        if (findnode == null) return null;
    
        bool isLeft = false;
        byte count = 0;
        if(findnode.left !=null) count+=1;
        if(findnode.right!=null) count+=1;

        GameObject removeObject = findnode.gameObject;
        
        if(findnode.Parent !=null){
            if (findnode.Value < findnode.Parent.Value) {
                isLeft = true;
                findnode.Parent.left = null;
            }
            else{
                isLeft = false;
                findnode.Parent.right = null;
            }
        }

        switch (count){
            case 0:
                //Root노드인데 자식노드도 없어서 지울 이유가 없는 유일한 케이스
                if (findnode == Root) return null;
            break;
            case 1:
                //왼쪽 노드가 없는 경우 -> 오른쪽 노드를 끌어 올림
                if(findnode.left == null)  OneChildNodeRemoveUpdate(ref findnode.Parent, ref findnode.right, ref removeObject, isLeft);
                //아닌 경우는 왼쪽 노드를 끌어 올림
                else if(findnode.right == null)  OneChildNodeRemoveUpdate(ref findnode.Parent, ref findnode.left, ref removeObject, isLeft);
            break;
            case 2:
                if(findnode.Parent !=null){
                    if(isLeft) findnode.Parent.left = findnode;
                    else findnode.Parent.right = findnode;
                }
                Node smallestNode = findSmallestNode(findnode.right);
                removeObject = removeNode(smallestNode.Value, smallestNode);
                _treeNodeCount += 1;
                findnode.Value = smallestNode.Value;
                findnode.GetComponent<Node>().SetNodeValue(findnode.Value);
            break;
        }
        _treeNodeCount-=1;
        
        return removeObject;

    }

    private void OneChildNodeRemoveUpdate(ref Node parent, ref Node child, ref GameObject removeObject, bool isLeft)
    {
        if (parent != null)
        {
            if (isLeft) parent.left = child;
            else parent.right = child;
        }

        child.Parent = parent;
        child.transform.position = removeObject.transform.position;
        if (parent != null)
        {
            child.transform.parent = parent.transform;
            //child.ConnectObject.transform.parent = parent.NodeObject.transform;
            //child.ConnectObject.transform.position = removeConnectObject.transform.position;
            //child.ConnectObject.transform.rotation = removeConnectObject.transform.rotation;
        }
        //root를 지우는 경우
        else
        {
            child.transform.parent = null;
            Root = child;
            //removeConnectObject = child.ConnectObject;
            AlgorithmTreeManager.RollBackStartNode();
        }
    }


    private Node findSmallestNode(Node node)
    {
        if (node.left == null) return node;
        return findSmallestNode(node.left);
    }
}