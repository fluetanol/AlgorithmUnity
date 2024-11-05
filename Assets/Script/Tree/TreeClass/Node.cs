using TMPro;
using UnityEngine;


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
    public TMP_Text DepthText;



    public void SetPositionOffset(float x) {
        transform.position = new Vector2(transform.position.x + x, transform.position.y);
        
    }


    public void SetCenterPos(){
        if(right == null && left == null) return;
        transform.position = new Vector2(
          (right.transform.position.x + left.transform.position.x) / 2,
          transform.position.y);
    }

    public void SetDepthText(int depth){
        DepthText.text = depth.ToString();
    }

    public void SetNodeValueText(int value){
        NodeValueText.text = value.ToString();
    }

}



public class AVLNode : Node{

}


