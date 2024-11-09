using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [Header("Game Setting")]
    public int SoundVolume;
    public int FrameRate;
    public bool isFixedFrameRate;   

    [Header("DOTWeen Setting")]
    public int TweenCapacity;
    public int SequenceCapacity;

    [Header("BGM Setting")]
    public List<AudioClip> Bgms;
    public AudioSource Source;


    void Awake(){
        if(isFixedFrameRate) Application.targetFrameRate = FrameRate;
        DOTween.Init().SetCapacity(TweenCapacity, SequenceCapacity);
    }

    void Start(){
    }
    void Update(){
        if (!Source.isPlaying){
            Source.clip = Bgms[Random.Range(0, Bgms.Count)];
            Source.Play();
        }
    }



}
