using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static InputManager current;
    [SerializeField] InputControlMap controlMap;
    
    Vector2 mouseVelocity;
    public bool isMouseDeltaAllow = true;
    private bool isMousePressed;


    void Awake(){
        current = this;
        controlMap = new InputControlMap();
    }
    
    void OnEnable(){
        controlMap.Enable();

        controlMap.TreeScene.MouseDelta.performed+= OnMouseDelta;
        controlMap.TreeScene.MouseButton.started += OnMouseClick;
        controlMap.TreeScene.MouseButton.canceled += OnMouseClick;

        controlMap.TreeScene.MouseDelta.Enable();
        controlMap.TreeScene.MouseButton.Enable();

    }

     void OnDisable() {
        controlMap.Disable();
        controlMap.TreeScene.MouseDelta.started-= OnMouseDelta;
        controlMap.TreeScene.MouseDelta.performed-= OnMouseDelta;

        controlMap.TreeScene.MouseButton.started -= OnMouseClick;
        controlMap.TreeScene.MouseButton.canceled -= OnMouseClick;

        controlMap.TreeScene.MouseDelta.Disable();
        controlMap.TreeScene.MouseButton.Disable();
    }

    void Update(){
        CloseFloatingUI();
    }

    void FixedUpdate() {
        if(isMousePressed && isMouseDeltaAllow){
            Camera.main.transform.position += new Vector3(mouseVelocity.x, mouseVelocity.y, 0) * Time.fixedDeltaTime;
        }
    }

    private void CloseFloatingUI(){
        if (EventSystem.current.currentSelectedGameObject == null){
            TreeUIManager.current.CloseNodeInfoUI(0.5f);
            TreeUIManager.current.CloseTextFieldPanelUI(0.5f);
        }
    }

    void OnMouseDelta(InputAction.CallbackContext context){
        if(!isMouseDeltaAllow) return;
        if(isMousePressed){
            mouseVelocity = -context.ReadValue<Vector2>();
            if (mouseVelocity.magnitude <= 5){
                mouseVelocity = Vector2.zero;
            }
        }
    }

    void OnMouseClick(InputAction.CallbackContext context){
        if(!isMouseDeltaAllow) return;
        if(context.ReadValue<float>()>=0.5f){
            //print("Mouse Pressed");
            isMousePressed = true;
        }else{
            StopAllCoroutines();
            StartCoroutine(CameraDecelerate(mouseVelocity));
            mouseVelocity = Vector2.zero;
            isMousePressed = false;
        }
    }   

    IEnumerator CameraDecelerate(Vector2 velocity){
        while(velocity.magnitude > 0){
            velocity = Vector2.Lerp(velocity, Vector2.zero, 0.2f);
            Camera.main.transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.fixedDeltaTime;
            if(velocity.magnitude <= 0.01f){
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void SetMouseDeltaAllow(bool isAllow){
        isMouseDeltaAllow = isAllow;
    }

}
