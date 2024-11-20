using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParent : MonoBehaviour
{
    public Transform myParent;

    public Vector3 position;

    public bool canFollowParent = true;

    // Start is called before the first frame update
    void Awake(){
        position = myParent.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(canFollowParent){
            transform.position = transform.position + (myParent.position - position);
            position = myParent.position;
        }
    }


    
}
