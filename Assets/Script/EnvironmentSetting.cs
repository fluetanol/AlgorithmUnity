using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public List<AudioClip> Bgms;
    public AudioSource Source;

    void Awake(){
        //Application.targetFrameRate = 120;
    }

    void Update(){
        if (!Source.isPlaying){
            Source.clip = Bgms[Random.Range(0, 2)];
            Source.Play();
        }
    }



}
