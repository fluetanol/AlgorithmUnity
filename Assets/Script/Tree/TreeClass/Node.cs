using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Node: MonoBehaviour
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

    [Header("Text")]
    public TMP_Text NodeValueText;

    [Header("Coponent")]
    public Button button;
    public Image image;

    public void SetNodeValue(int value){
        Value = value;  
        NodeValueText.text = value.ToString();
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
        Camera.main.DOCameraMove(position, deltaTime);
        Camera.main.DOCameraZoom(2.5f, deltaTime);
        TreeUIManager.ShowNodeInfoUI(0.3f, deltaTime);
    }


}


