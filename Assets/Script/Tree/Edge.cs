using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public Transform Node1;
    public Transform Node2;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Node1 != null && Node2 != null)
        {
            _lineRenderer.SetPosition(0, Node1.position);
            _lineRenderer.SetPosition(1, Node2.position);
        }
    }

    public void SetEdgeNode(Transform node1, Transform node2)
    {
        Node1 = node1;
        Node2 = node2;
    }
    
}