
using UnityEngine;

public sealed class AVLTree : BinaryTree
{
    public AVLTree(GameObject rootObject, int rootValue)
    {
        Root = new(){
            Value = rootValue,
            NodeObject = rootObject,
            Depth = 0,
        };
        SetRootValue(rootValue);
        _originNodeColor = Root.NodeObject.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        _treeNodeCount += 1;
        _height += 1;
    }

    public override bool Add(Node node) {
        bool isAdd = addNode(node, Root, 1);
        if(isAdd){
            int bf = UpdateDepth(ref Root);
            if (Mathf.Abs(bf) >= 2) RightRotation(Root);
        }
        return isAdd;
    }

    private bool addNode(Node node, Node currentNode, int depth)
    {
        bool isFind = true;
        if (depth > _height) _height = depth;
        if (node.Value < currentNode.Value)
        {
            if (currentNode.left == null) PlaceNodeObject(ref node, ref currentNode, true, depth);
            else{
                currentNode = currentNode.left;
                depth += 1;
                isFind = addNode(node, currentNode, depth);
                if (isFind) {
                    int bf = UpdateDepth(ref currentNode);
                    if(Mathf.Abs(bf)>=2){
                        RightRotation(currentNode);
                    }
                }
            }
        }
        else if (node.Value > currentNode.Value)
        {
            if (currentNode.right == null) PlaceNodeObject(ref node, ref currentNode, false, depth);
            else{
                currentNode = currentNode.right;
                depth += 1;
                isFind = addNode(node, currentNode, depth);
                if (isFind) {
                    int bf = UpdateDepth(ref currentNode);
                    if (Mathf.Abs(bf) >= 2){
                        //LeftRotation(currentNode);
                    }
                }
            }
        }
        else if (node.Value == currentNode.Value) return false;
        //Debug.Log(currentNode.Value);
        return isFind;
    }

    private void RightRotation(Node currentNode){
        PlaceRotationObject(currentNode);

        Node node = currentNode.left.right;
        currentNode.left.right = currentNode;
        currentNode.left.Parent = currentNode.Parent;
        if (currentNode.Parent != null){
            if(currentNode.Parent.Value < currentNode.left.Value) currentNode.Parent.right = currentNode.left;
            else currentNode.Parent.left = currentNode.left;
        }
        currentNode.Parent = currentNode.left;
        //현 노드의 좌노드 설정
        currentNode.left = node;

        if (currentNode.left != null) currentNode.left.Parent = currentNode;
        if (currentNode == Root){
            Root = currentNode.Parent;
            AlgorithmTreeManager.Instance.RollBackStartNode();
            if(Root.Parent == null) Debug.Log("good");
        }
        currentNode = currentNode.Parent;
        
        UpdateDepth(ref currentNode.right);
        UpdateDepth(ref currentNode);
    }


    private void LeftRotation(ref Node currentNode){

    }


    private void PlaceRotationObject(Node currentNode){
        currentNode.left.NodeObject.transform.SetParent(currentNode.NodeObject.transform.parent);
        //currentNode.left.left.NodeObject.transform.SetParent(currentNode.left.NodeObject.transform);

        Vector3 currentPosition = currentNode.NodeObject.transform.position;
        GameObject currentConnectObject = currentNode.ConnectObject;
        currentNode.ConnectObject = currentNode.left.ConnectObject;
        currentNode.left.ConnectObject = currentConnectObject;

        var ConnectInfo = currentNode.ConnectObject.GetComponent<NodeConnectObejctInfo>();
        var ParentNodeInfo = currentNode.left.NodeObject.GetComponent<NodeObjectInfo>();
        currentNode.NodeObject.transform.parent = currentNode.left.NodeObject.transform;
        currentNode.ConnectObject.transform.parent = currentNode.left.NodeObject.transform;

        currentNode.ConnectObject.transform.eulerAngles = -currentNode.ConnectObject.transform.eulerAngles;
        Vector3 a = ConnectInfo.transform.position;
        Vector3 b = ConnectInfo.StartPoint.position;
        ConnectInfo.transform.position = ParentNodeInfo.rightNodePoint.position + (a - b);
        currentNode.NodeObject.transform.position = ConnectInfo.EndPoint.position;

        if(currentNode.left.right != null) {
            currentNode.left.right.NodeObject.transform.parent = currentNode.NodeObject.transform;
            currentNode.left.right.ConnectObject.transform.parent = currentNode.NodeObject.transform;
            currentNode.left.right.ConnectObject.transform.eulerAngles = - currentNode.left.right.ConnectObject.transform.eulerAngles;
            ConnectInfo = currentNode.left.right.ConnectObject.GetComponent<NodeConnectObejctInfo>();
            ParentNodeInfo = currentNode.NodeObject.GetComponent<NodeObjectInfo>();
            a = ConnectInfo.transform.position;
            b = ConnectInfo.StartPoint.position;
            ConnectInfo.transform.position = ParentNodeInfo.leftNodePoint.position + (a - b);
            currentNode.left.right.NodeObject.transform.position = ConnectInfo.EndPoint.position;
            
        }


        currentNode.left.NodeObject.transform.position = currentPosition;
    }





    public override Node Find(int Value)
    {
        throw new System.NotImplementedException();
    }

    public override bool isExist(int Value)
    {
        throw new System.NotImplementedException();
    }

    public override (GameObject, GameObject) Remove(int Value)
    {
        throw new System.NotImplementedException();
    }

    protected override void PlaceNodeObject(ref Node node, ref Node currentNode, bool isLeft, float depth)
    {
        base.PlaceNodeObject(ref node, ref currentNode, isLeft, depth);
        var nodeInfo = node.NodeObject.GetComponent<NodeObjectInfo>();
        node.Depth = 0;
        nodeInfo.DepthText.text = "0";
    }

    private int UpdateDepth(ref Node node){
        var nodeInfo = node.NodeObject.GetComponent<NodeObjectInfo>();

        if(node.left == null && node.right == null){
            node.Depth = node.Parent.Depth - 1;
            node.BF = 0;
        }

        else if(node.left == null) {
            node.Depth = node.right.Depth + 1;
            node.BF = -node.right.Depth -1;
        }
        else if(node.right==null) {
            node.Depth = node.left.Depth + 1;
            node.BF = node.left.Depth +1;
        }
        else if(node.left.Depth>=node.right.Depth) {
            node.Depth = node.left.Depth +1;
            node.BF = node.left.Depth - node.right.Depth;

        }
        else if(node.left.Depth<node.right.Depth) {
            node.Depth = node.right. Depth+1;
            node.BF = node.left.Depth - node.right.Depth;
        }
        
        nodeInfo.DepthText.text = node.BF+"";
        return node.BF;
    }

}
