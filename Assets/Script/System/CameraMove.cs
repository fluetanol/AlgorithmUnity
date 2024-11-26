using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public static class CameraMoveExtension{
    public static Tween DOCameraMove(this Camera camera, Vector3 targetPos, float seconds, Ease ease = Ease.InOutQuad){
        return camera.transform.DOMove(targetPos,seconds).SetEase(ease);
    }

    public static Tweener DOCameraZoom(this Camera camera, float targetSize, float seconds, Ease ease = Ease.InOutQuad){
        if(camera.orthographicSize == targetSize) return null;
        return camera.DOOrthoSize(targetSize, seconds).SetEase(ease);   
    }
}