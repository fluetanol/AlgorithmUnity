
using UnityEngine;

public class TestCode : MonoBehaviour
{

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.DOCameraMove(Camera.main.transform.position + Vector3.right, 1f);
    }

}
