using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStageHelpPopup : UIPopup
{
    enum Buttons
    {
        StageHelpPopupDeleteButton,
    }



    enum GameObjects
    {
        StageInfoPopupText
    }

    int _score = 0;

    void Start()
    {
        Init();
    }

    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log("Button Click!");
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        // Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        // Bind<Image>(typeof(Images));

        // GetText((int) Texts.ScoreText).text = "Bind Test";
        
        //GetButton((int) Buttons.MakeButton).onClick.AddListener(() => OnGateMake(null));

        
        GameObject popupDeleteObject = GetButton((int) Buttons.StageHelpPopupDeleteButton).gameObject;
        //popupDeleteButton.onClick.AddListener(OnPopupDelete);
        // Get<GameObject>((int) GameObjects.StageInfoPopupText).GetComponent<TextMeshProUGUI>().text = $"Test";

        BindEvent(popupDeleteObject, OnPopupDelete, Define.UIEvent.Click);
    
    }

    public void OnPopupDelete(PointerEventData data)
    {
        gameObject.SetActive(false);
    }
}