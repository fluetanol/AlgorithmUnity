using UnityEngine;


public sealed class BinarySearchTree : BinaryTree{
    public BinarySearchTree(GameObject rootObject, int rootValue){
        Root = new(){
            Value = rootValue,
            NodeObject = rootObject,
        };
        SetRootValue(rootValue);
        _originNodeColor = Root.NodeObject.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        _treeNodeCount += 1;
        _height += 1;
    }

    public override bool Add(Node node) =>addNode(node, Root, 1);
    public override (GameObject, GameObject) Remove(int Value) => removeNode(Value, Root);
    public override Node Find(int Value) => findNode(Value, Root);
    public override bool isExist(int Value){
        Node node = Find(Value);
        if (node != null)
        {
            showFindNode(ref node);
            return true;
        }
        else return false;
    }

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
        if(Value == node.Value)  return node;
        if(Value < node.Value) find = findNode(Value, node.left);
        else if(Value > node.Value) find = findNode(Value, node.right);
        return find;
    }

    private void showFindNode(ref Node node){
        if (_recentFindNode != null) _recentFindNode.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _originNodeColor);
        node.NodeObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.yellow);
        _recentFindNode = node;
    }

    private (GameObject, GameObject) removeNode(int value, Node node){
        ResetRecentNode();
        Node findnode = findNode(value, node);
        if (findnode == null) return (null, null);
        
        GameObject removeObject = null;
        GameObject removeConnectObject = null;
        bool isLeft = false;
        byte count = 0;
        if(findnode.left !=null) count+=1;
        if(findnode.right!=null) count+=1;

        removeObject = findnode.NodeObject;
        removeConnectObject = findnode.ConnectObject;
        
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
                if (findnode == Root) return (null, null);
            break;
            case 1:
                //왼쪽 노드가 없는 경우 -> 오른쪽 노드를 끌어 올림
                if(findnode.left == null)  OneChildNodeRemoveUpdate(ref findnode.Parent, ref findnode.right, ref removeObject,ref removeConnectObject, isLeft);
                //아닌 경우는 왼쪽 노드를 끌어 올림
                else if(findnode.right == null)  OneChildNodeRemoveUpdate(ref findnode.Parent, ref findnode.left, ref removeObject, ref removeConnectObject, isLeft);
            break;
            case 2:
                if(findnode.Parent !=null){
                    if(isLeft) findnode.Parent.left = findnode;
                    else findnode.Parent.right = findnode;
                }
                Node smallestNode = findSmallestNode(findnode.right);
                (removeObject, removeConnectObject) = removeNode(smallestNode.Value, smallestNode);
                _treeNodeCount += 1;
                findnode.Value = smallestNode.Value;
                findnode.NodeObject.GetComponent<NodeObjectInfo>().NodeValueText.text = findnode.Value.ToString();
            break;
        }
        _treeNodeCount-=1;
        return (removeObject, removeConnectObject);
    }

    private void OneChildNodeRemoveUpdate(ref Node parent, ref Node child, ref GameObject removeObject, ref GameObject removeConnectObject, bool isLeft)
    {
        if (parent != null)
        {
            if (isLeft) parent.left = child;
            else parent.right = child;
        }

        child.Parent = parent;
        child.NodeObject.transform.position = removeObject.transform.position;
        if (parent != null)
        {
            child.NodeObject.transform.parent = parent.NodeObject.transform;
            child.ConnectObject.transform.parent = parent.NodeObject.transform;
            child.ConnectObject.transform.position = removeConnectObject.transform.position;
            child.ConnectObject.transform.rotation = removeConnectObject.transform.rotation;
        }
        //root를 지우는 경우
        else
        {
            child.NodeObject.transform.parent = null;
            Root = child;
            removeConnectObject = child.ConnectObject;
            AlgorithmTreeManager.RollBackStartNode();
        }
    }


    private Node findSmallestNode(Node node)
    {
        if (node.left == null) return node;
        return findSmallestNode(node.left);
    }
}