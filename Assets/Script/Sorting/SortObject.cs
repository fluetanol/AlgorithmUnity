using System;
using UnityEngine;

public class SortObject : MonoBehaviour, IComparable<SortObject>
{
    public int value;

    private SpriteRenderer _spriteRenderer;

    private void Awake(){
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Color GetColor()=> _spriteRenderer.color;
    public void SetColor(Color color)=> _spriteRenderer.color = color;
    public void SetSize(Vector3 size) => transform.localScale = size;

    public int CompareTo(SortObject other)
    {
        if(other == null) return 1;
        return value.CompareTo(other.value);
    }

    //opertor overloading
    public static bool operator <=(SortObject a, SortObject b){
        return a.CompareTo(b) <= 0;
    }
    public static bool operator >=(SortObject a, SortObject b) {
        return a.CompareTo(b) >= 0;
    } 
    public static bool operator <(SortObject a, SortObject b) {
        return a.CompareTo(b) < 0;
    }
    public static bool operator >(SortObject a, SortObject b) {
        return a.CompareTo(b) > 0;
    }
    
}
