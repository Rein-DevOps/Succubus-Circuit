using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGateButton : UIScene
{
    
    public List<Switch> switches = new();

    [SerializeField]
    TextMeshProUGUI _text;

    enum Buttons
    {
        MakeButton,
        DeleteButton,
        TestButton,
        StageInfoButton,
        MenuButton,
    }

    enum Images
    {
        MakeIcon,
        DeleteIcon,
        TestIcon,
        StageInfoIcon
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
        Bind<Image>(typeof(Images));

        // GetText((int) Texts.ScoreText).text = "Bind Test";
        
        //GetButton((int) Buttons.MakeButton).onClick.AddListener(() => OnGateMake(null));

        GameObject makeObject = GetButton((int) Buttons.MakeButton).gameObject;
        GameObject DeleteObject = GetButton((int) Buttons.DeleteButton).gameObject;
        GameObject TestObject = GetButton((int) Buttons.TestButton).gameObject;
        GameObject StageInfoObject = GetButton((int) Buttons.StageInfoButton).gameObject;
        GameObject menuObject = GetButton((int) Buttons.MenuButton).gameObject;

        BindEvent(makeObject, OnGateMake, Define.UIEvent.Click);
        BindEvent(DeleteObject, OnObjectDelete, Define.UIEvent.Click);
        BindEvent(TestObject, OnTest, Define.UIEvent.Click);
        BindEvent(StageInfoObject, OnStageInfo, Define.UIEvent.Click);
        BindEvent(menuObject, OnMenuActive, Define.UIEvent.Click);
    }

    public void OnGateMake(PointerEventData data)
    {
        Debug.Log("Gate Make Button Called");
        GameManager.Circuit.InstantiateCircuit();
    }

    public void OnObjectDelete(PointerEventData data)
    {
        Debug.Log("Gate Delete Button Called");
        GameManager.Circuit.Delete();
    }

    public void OnTest(PointerEventData data)
    {
        foreach(var Switch in switches)
        {
            Switch.LocalTest();
        }
    }

    public void OnStageInfo(PointerEventData data)
    {
        Transform stageInfo = transform.Find("StageInfoPopup");
        
        stageInfo.gameObject.SetActive(true);
        AudioSource audioSource = stageInfo.GetComponent<AudioSource>();
        audioSource.time = 0.6f;
        audioSource.Play();;
    }

    public void OnMenuActive(PointerEventData data)
    {
        Transform stageMenu = transform.Find("StageMenu");
        stageMenu.gameObject.SetActive(true);
    }
}
