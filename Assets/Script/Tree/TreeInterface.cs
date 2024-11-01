using UnityEngine;



public interface ITreeManage
{
    public void SetNewTree(int num);
    public int GetTreeNodeCount();
    public void ResetRecentNode();
    public void SetRootValue(int value);
}

public interface INodeManage
{
    public Node NewNode(int value);
    public bool AddNode(Node node);
    public (GameObject, GameObject) RemoveNode(int value);
    public bool IsExistNode(int value);
}

public interface ITreeTraversal
{
    public void SetTraversalMode(TraversalMode? mode);
    public void EnumerateStepTraversal();
    public void EnumerateCoroutineTraversal();
}
