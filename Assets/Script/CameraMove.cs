using DG.Tweening;
using UnityEngine;

public static class CameraMoveExtension{
    public static Tween DOCameraMove(this Camera camera, Vector3 targetPos, float seconds, Ease ease = Ease.InOutQuad){
        return camera.transform.DOMove(targetPos,seconds).SetEase(ease);
    }

    public static Tween DOCameraZoom(this Camera camera, float targetSize, float seconds, Ease ease = Ease.InOutQuad){
        return camera.DOOrthoSize(targetSize, seconds).SetEase(ease);   
    }
}