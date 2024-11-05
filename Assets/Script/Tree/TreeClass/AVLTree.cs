
using Unity.VisualScripting;
using UnityEditor.UnityLinker;
using UnityEngine;

public sealed class AVLTree : BinaryTree
{
    public AVLTree(Node rootNode, int rootValue)
    {
        Root = new(){
            Value = rootValue,
            Depth = 0,
        };
        SetRootValue(rootValue);
        _originNodeColor = Root.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        _treeNodeCount += 1;
        _height += 1;
    }

    public override bool Add(Node node, Edge edge) {
        bool isAdd = addNode(node, Root, 1);
        if(isAdd){
            int bf = UpdateDepth(ref Root);
            if (Mathf.Abs(bf) >= 2) {
                //if(Root.left!=null)  RightRotation(Root, ref Root.left, ref Root.left.right, true);
                if(Root.right!=null) Rotation(Root, ref Root.right, ref Root.right.left, false);       
            }
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
                       // RightRotation(currentNode, ref currentNode.left, ref currentNode.left.right, true);
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
                        Debug.Log(currentNode.Value + " " + bf);
                        Rotation(currentNode, ref currentNode.right, ref currentNode.right.left, false);
                    }
                }
            }
        }
        else if (node.Value == currentNode.Value) return false;
        return isFind;
    }

    private void RightRotation(Node currentNode ){
        PlaceRotationObject(currentNode);
        Node node = currentNode.left.right;
        currentNode.left.right = currentNode;
        currentNode.left.Parent = currentNode.Parent;
        if (currentNode.Parent != null){
            if(currentNode.Parent.Value < currentNode.left.Value) currentNode.Parent.right = currentNode.left;
            else currentNode.Parent.left = currentNode.left;
        }
        currentNode.Parent = currentNode.left;
        currentNode.left = node;

        if (currentNode.left != null) currentNode.left.Parent = currentNode;
        if (currentNode == Root){
            Root = currentNode.Parent;
            AlgorithmTreeManager.RollBackStartNode();
            if(Root.Parent == null) Debug.Log("good");
        }
        currentNode = currentNode.Parent;
        
        UpdateDepth(ref currentNode.right);
        UpdateDepth(ref currentNode);
    }



    private void Rotation(Node currentNode, ref  Node currentleft, ref Node currentLeftRight, bool isRR)
    {
        currentNode.GetComponent<SpriteRenderer>().material.SetColor("_EmissionColor", Color.blue);
        PlaceRotationObject(currentNode, ref currentleft, ref currentLeftRight, isRR);

        Node node = currentLeftRight;
        currentLeftRight = currentNode;
        currentleft.Parent = currentNode.Parent;
        if (currentNode.Parent != null)
        {
            if (currentNode.Parent.Value < currentleft.Value) currentNode.Parent.right = currentleft;
            else currentNode.Parent.left = currentleft;
        }
        currentNode.Parent = currentleft;
        currentleft = node;

        if (currentleft != null) currentleft.Parent = currentNode;
        if (currentNode == Root)
        {
            Root = currentNode.Parent;
            AlgorithmTreeManager.RollBackStartNode();
            if (Root.Parent == null) Debug.Log("good");
        }

        currentNode = currentNode.Parent;

        if (isRR) UpdateAllDepth(ref currentNode.right);
        else UpdateAllDepth(ref currentNode.left);

        /*
        if (isRR) UpdateDepth(ref currentNode.right);
        else UpdateDepth(ref currentNode.left);

        UpdateDepth(ref currentNode);
        */
    }


    private void PlaceRotationObject(Node currentNode){
        /*
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
        */
    }

 
    private void PlaceRotationObject(Node currentNode, ref Node currentleft, ref Node currentleftright, bool isRR)
    {
        /*
        currentleft.NodeObject.transform.SetParent(currentNode.NodeObject.transform.parent);

        Vector3 currentPosition = currentNode.NodeObject.transform.position;
        GameObject currentConnectObject = currentNode.ConnectObject;
        currentNode.ConnectObject = currentleft.ConnectObject;
        currentleft.ConnectObject = currentConnectObject;

        var ConnectInfo = currentNode.ConnectObject.GetComponent<NodeConnectObejctInfo>();
        var ParentNodeInfo = currentleft.NodeObject.GetComponent<NodeObjectInfo>();
        currentNode.NodeObject.transform.parent = currentleft.NodeObject.transform;
        currentNode.ConnectObject.transform.parent = currentleft.NodeObject.transform;

        currentNode.ConnectObject.transform.eulerAngles = -currentNode.ConnectObject.transform.eulerAngles;
        Vector3 a = ConnectInfo.transform.position;
        Vector3 b = ConnectInfo.StartPoint.position;
        if(isRR) ConnectInfo.transform.position = ParentNodeInfo.rightNodePoint.position + (a - b);
        else ConnectInfo.transform.position = ParentNodeInfo.leftNodePoint.position + (a - b);
        currentNode.NodeObject.transform.position = ConnectInfo.EndPoint.position;


        if (currentleftright != null)
        {

            currentleftright.NodeObject.transform.parent = currentNode.NodeObject.transform;
            currentleftright.ConnectObject.transform.parent = currentNode.NodeObject.transform;
            currentleftright.ConnectObject.transform.eulerAngles = -currentleftright.ConnectObject.transform.eulerAngles;
            ConnectInfo = currentleftright.ConnectObject.GetComponent<NodeConnectObejctInfo>();
            ParentNodeInfo = currentNode.NodeObject.GetComponent<NodeObjectInfo>();
            a = ConnectInfo.transform.position;
            b = ConnectInfo.StartPoint.position;

            if (isRR) ConnectInfo.transform.position = ParentNodeInfo.leftNodePoint.position + (a - b);
            else ConnectInfo.transform.position = ParentNodeInfo.rightNodePoint.position + (a - b);
            currentleftright.NodeObject.transform.position = ConnectInfo.EndPoint.position;

        }
        currentleft.NodeObject.transform.position = currentPosition;
            */
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
        var nodeInfo = node.GetComponent<Node>();
        node.Depth = 0;
        nodeInfo.SetDepthText(0);
    }


    private void UpdateAllDepth(ref Node node){
        UpdateDepth(ref node);
        if (node == Root) return;
        UpdateAllDepth(ref node.Parent);
    }

    private int UpdateDepth(ref Node node){
        var nodeInfo = node.GetComponent<Node>();

        Debug.Log(node.Value);
        
        if (node.left == null && node.right == null){
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
        else if(node.left.Depth>node.right.Depth) {
            node.Depth = node.left.Depth +1;
            node.BF = node.left.Depth - node.right.Depth;

        }
        else if(node.left.Depth<node.right.Depth) {
            node.Depth = node.right. Depth+1;
            node.BF = node.left.Depth - node.right.Depth;
        }
        
        nodeInfo.SetDepthText(node.BF);
        return node.BF;
    }

}
