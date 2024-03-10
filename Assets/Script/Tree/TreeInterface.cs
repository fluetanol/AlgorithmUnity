using UnityEngine.Rendering;
using UnityEngine;

public interface TreeInterface{
    public int GetNodeCount();
    public void PostOrderTraversal();
    public void PreorderTraversal();
    public void InorderTraversal();
    public void LevelorderTraversal();
    //bool은 더이상 sorting을 안해도 되는지 해야하는지 여부를 반환함
    //더 해야하는 경우 false, 끝난 경우 true 반환.
    public void UpdateInorderTraversal(ref Node node);
    public void UpdatePreorderTraversal(ref Node node);
    public void UpdatePostorderTraversal(ref Node node);
    public void UpdateLevelorderTraversal(ref Node node);
}


