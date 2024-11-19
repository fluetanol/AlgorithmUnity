using UnityEngine;

public class Edge : MonoBehaviour
{
    private     LineRenderer    _lineRenderer;
    public      Node       Node1;
    public      Node       Node2;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, Vector3.zero);
        _lineRenderer.SetPosition(1, Vector3.zero);
    }

    private void Update()
    {
        if (Node1.gameObject.activeSelf && Node2.gameObject.activeSelf)
        {
            _lineRenderer.SetPosition(0, Node1.transform.position);
            _lineRenderer.SetPosition(1, Node2.transform.position);
        }else{
            ObjectPool.DestoyPoolObject(this.gameObject, ObjectPoolType.Edge);
        }
    }

    public void SetEdgeNode(Node node1, Node node2)
    {
        Node1 = node1;
        Node2 = node2;
    }
    
}
