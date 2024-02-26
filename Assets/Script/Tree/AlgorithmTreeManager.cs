
using UnityEngine;

public class AlgorithmTreeManager : MonoBehaviour
{
    public GameObject NodePrefab;
    public GameObject ConnectPrefab;
    public static AlgorithmTreeManager Instance;
    public static BinaryTree BinaryTree;

    // Start is called before the first frame update
    void Awake(){
        Instance = this;
        BinaryTree = new();
        GameObject rootObject = Instantiate(NodePrefab);
        rootObject.transform.position = Vector3.zero;
        rootObject.GetComponent<NodeObjectInfo>().NodeValueText.text = 0.ToString();
        BinaryTree.Root.NodeObject = rootObject;
        BinaryTree.Root.Value = 0;
    }

}
