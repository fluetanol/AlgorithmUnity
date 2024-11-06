using System;
using System.Collections;
using System.Collections.Generic;
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

    public Vector3 position;



    public void SetCenterPos(){
        if(right == null || left == null) return;
        transform.position = new Vector2(
          (right.transform.position.x + left.transform.position.x) / 2,
          transform.position.y);
    }

    public void SetNodeValue(int value){
        Value = value;  
        NodeValueText.text = value.ToString();
    }

    public IEnumerator PositionMove(Vector3 targetPos, float seconds){
        Vector3 currentPos = transform.position;
        float elapsedTime = 0;
        while (elapsedTime < seconds){
            transform.position = Vector3.Lerp(currentPos, targetPos, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
    }

}



public class AVLNode : Node{

}


