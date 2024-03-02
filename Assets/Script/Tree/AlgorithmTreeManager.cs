
using UnityEngine;

public class AlgorithmTreeManager : MonoBehaviour
{
    public GameObject NodePrefab;
    public GameObject ConnectPrefab;
    public static AlgorithmTreeManager Instance;
    public static BinaryTree BinaryTree;
    public int InitializeValue = 2;

    // Start is called before the first frame update
    void Awake(){
        Instance = this;
        BinaryTree = new();
        GameObject rootObject = Instantiate(NodePrefab);
        rootObject.transform.position = Vector3.zero;
        rootObject.GetComponent<NodeObjectInfo>().NodeValueText.text = InitializeValue.ToString();
        BinaryTree.Root.NodeObject = rootObject;
        BinaryTree.Root.Value = InitializeValue;
    }

}
