using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Setting : MonoBehaviour
{

    public enum FrameRate
    {
        _30 = 30,
        _60 = 60,
        _120 = 120
    }

    public enum Resolution
    {
        _1920x1080 = 0,
        _1600x900 = 1,
        _1280x720 = 2,
        _800x600 = 3
    }

    [Header("Game Setting")]
    [Range(0,1)] public float SoundVolume = 0.5f;
    public FrameRate frameRate = FrameRate._60;
    public Resolution resolution = Resolution._1920x1080;
    public bool isFixedFrameRate;   

    [Header("DOTWeen Setting")]
    public int TweenCapacity;
    public int SequenceCapacity;

    [Header("BGM Setting")]
    public List<AudioClip> Bgms;
    public AudioSource Source;
    public bool SoundOn;


    void Awake(){
        if(isFixedFrameRate) Application.targetFrameRate = (int)frameRate;
        DOTween.Init().SetCapacity(TweenCapacity, SequenceCapacity);
    }

    void Update(){
        SoundControl();
        VolumeControl();    
    }

    void SoundControl(){
        if (SoundOn && !Source.isPlaying)
        {
            Source.clip = Bgms[Random.Range(0, Bgms.Count)];
            Source.Play();
        }
        else if (!SoundOn && Source.isPlaying)
        {
            Source.Stop();
        }
    }

    void VolumeControl(){
        if (Source.volume != SoundVolume && Source.isPlaying)
            Source.volume = SoundVolume;
    }
}
