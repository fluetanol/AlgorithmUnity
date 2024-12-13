using System;
using UnityEngine;

public class SortObject : MonoBehaviour, IComparable<SortObject>
{
    public int value{
        get;
        private set;
    }

    private SpriteRenderer _spriteRenderer;

    private void Awake(){
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public Color GetColor()=> _spriteRenderer.color;
    public void SetColor(Color color)=> _spriteRenderer.color = color;
    public void SetSize(Vector3 size) => transform.localScale = size;

    public int CompareTo(SortObject other)
    {
        if(other == null) return 1;
        return value.CompareTo(other.value);
    }

    public void Set(int value){
        this.value = value;
        name = value.ToString();
        transform.localScale = new Vector3(1, value, 1);
        transform.position = new Vector3(value, 0, 0);
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
