using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    InputAction inputAction;

    void Start(){

    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            TreeUIManager.CloseNodeInfoUI(0.5f);
        }

        
    }



}
