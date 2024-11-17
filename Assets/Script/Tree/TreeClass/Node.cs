using DG.Tweening;
using UnityEngine;


public class Node: NodeUI
{
    [Header("Node Info")]
    public int BF = 0;
    public int Depth = 0;
    public int Value;
    public bool isLeft = false; //부모 기준 현재 노드가 왼쪽인지 오른쪽인지

    [Header("Node")]
    public Node Parent = null;
    public Node right = null;
    public Node left = null;

    public override void SetNodeValue(int value){
        base.SetNodeValue(value);
        Value = value;
    }

    public void SetCenterPos(){
        if (right == null || left == null) return;
        Vector3 position = new Vector2((right.transform.position.x + left.transform.position.x) / 2, transform.position.y);
        transform.DOMove(position, 0.5f);
    }

    public void PositionMove(ref Sequence sequence, Vector3 targetPos, float seconds){
        sequence.Append(transform.DOMove(targetPos,seconds).SetEase(Ease.InOutQuad));
    }

    public void OnNodePress(float deltaTime){
        Vector3 position = this.transform.position;
        position.z = Camera.main.transform.position.z;
        TreeUIManager.current.FocusNode(position, deltaTime);
        TreeUIManager.current.ShowNodeInfoUI(deltaTime);
        NodeInfoPanel.current.SetNodeInfo(Value, Depth, BF);
    }
}


