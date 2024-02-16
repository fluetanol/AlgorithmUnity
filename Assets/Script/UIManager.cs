using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance{
        get{
            if(_instance == null)
                _instance = new();
            return _instance;
        }
    }
    private static UIManager _instance;

    void OnEnable() => DontDestroyOnLoad(gameObject);


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
