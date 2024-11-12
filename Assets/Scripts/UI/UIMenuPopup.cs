using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenuPopup : UIPopup
{
    enum Buttons
    {
        MenuDeleteButton,
        MenuHomeButton,
    }

    int _score = 0;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        
        GameObject popupDeleteObject = GetButton((int) Buttons.MenuDeleteButton).gameObject;
        GameObject popupHomeObject = GetButton((int) Buttons.MenuHomeButton).gameObject;


        BindEvent(popupDeleteObject, OnPopupDelete, Define.UIEvent.Click);
        BindEvent(popupHomeObject, OnHome, Define.UIEvent.Click);
    
    }

    public void OnHome(PointerEventData data)
    {
        // GameManager.SceneChange(Define.Scene.Menu);
        
    }

    public void OnPopupDelete(PointerEventData data)
    {
        gameObject.SetActive(false);
    }


}
