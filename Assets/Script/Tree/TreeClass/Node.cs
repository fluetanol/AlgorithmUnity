using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Node
{
    public int BF = 0;
    public int Depth = 0;
    public Node right = null;
    public Node left = null;
    public Node Parent = null;
    public int Value;
    public GameObject NodeObject;
    public GameObject ConnectObject;
}
