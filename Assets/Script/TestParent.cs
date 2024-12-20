using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParent : MonoBehaviour
{
    public Transform MyParent;
    public Vector3   position;
    public bool      canFollowParent = true;

    // Start is called before the first frame update
    void Awake(){
        position = MyParent.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(canFollowParent){
            transform.position = transform.position + (MyParent.position - position);
            position = MyParent.position;
        }
    }


    
}
