using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public enum ETextFieldUIType{
    InputField,
    ConfirmButton,
    OpenButton
}

public class TextFieldPanel : PanelBlocking
{
    public TMP_InputField   InputField;
    public Button           ConfirmButton;
    public Button           OpenButton;
    public Vector2          OriginAnchor;

    void Awake(){
       // OriginAnchor = GetComponent<RectTransform>().anchoredPosition;
        print(OriginAnchor);
    }

    public void ResetAllListener(){
        ConfirmButton.onClick.RemoveAllListeners();
        OpenButton.onClick.RemoveAllListeners();
        InputField.onValueChanged.RemoveAllListeners();
    }

    public void ResetUIListener(params ETextFieldUIType[] UItypes){
        foreach(var uitype in UItypes){
            switch(uitype){
                case ETextFieldUIType.InputField:
                    InputField.onValueChanged.RemoveAllListeners();
                    break;
                case ETextFieldUIType.ConfirmButton:
                    ConfirmButton.onClick.RemoveAllListeners();
                    break;
                case ETextFieldUIType.OpenButton:
                    OpenButton.onClick.RemoveAllListeners();
                    break;
            }
        }
    }

    public void onValueChangedListener(UnityAction<string> listener){
        InputField.onValueChanged.AddListener(listener);
    }

    public void onClickListener(UnityAction listener){
        ConfirmButton.onClick.AddListener(listener);
    }

    public void ResetAllValue(){
        InputField.text = "";
    }
}
