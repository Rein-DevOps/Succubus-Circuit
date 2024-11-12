using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGateButton : UIScene
{
    
    public List<Switch> switches = new();
    public GameObject SuccessPanel;
    public OutCome outCome;
    public List<Switch> swts = new();

    public List<AudioClip> successAudioClips = new();
    public List<AudioClip> failAudioClips = new();
    AudioSource audioSource;

    [SerializeField]
    TextMeshProUGUI _text;

    enum Buttons
    {
        MakeButton,
        DeleteButton,
        TestButton,
        StageInfoButton,
        MenuButton,
        HelpButton,
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
        audioSource = GetComponent<AudioSource>();
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
        GameObject helpObject = GetButton((int) Buttons.HelpButton).gameObject;

        BindEvent(makeObject, OnGateMake, Define.UIEvent.Click);
        BindEvent(DeleteObject, OnObjectDelete, Define.UIEvent.Click);
        BindEvent(TestObject, OnTest, Define.UIEvent.Click);
        BindEvent(StageInfoObject, OnStageInfo, Define.UIEvent.Click);
        BindEvent(menuObject, OnMenuActive, Define.UIEvent.Click);
        BindEvent(helpObject, OnHelpActive, Define.UIEvent.Click);
    }

    public void OnGateMake(PointerEventData data)
    {
        Debug.Log("Gate Make Button Called");
        // GameManager.Circuit.InstantiateCircuit();
    }

    public void OnObjectDelete(PointerEventData data)
    {
        Debug.Log("Gate Delete Button Called");
        // GameManager.Circuit.Delete();
    }

    public void OnTest(PointerEventData data)
    {
        GameManager.Circuit.Test();
        if (GameManager.Circuit.IsCorrect)
        {
            SuccessPanel.SetActive(true);
            if (successAudioClips.Count > 0)
            {
                int randomIndex = Random.Range(0, successAudioClips.Count);
                audioSource.clip = successAudioClips[randomIndex];
                audioSource.Play();
            }
        }
        else
        {
            // 실패 시 failAudioClips 중 랜덤 오디오 재생
            if (failAudioClips.Count > 0)
            {
                int randomIndex = Random.Range(0, failAudioClips.Count);
                audioSource.clip = failAudioClips[randomIndex];
                audioSource.Play();
            }
        }
        outCome.TestEnd();
        foreach (var swt in swts)
        {
            swt.TestEnd();
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
    public void OnHelpActive(PointerEventData data)
    {
        Transform helpPopup = transform.Find("StageHelpPopup");
        helpPopup.gameObject.SetActive(true);
        AudioSource audioSource = helpPopup.GetComponent<AudioSource>();
        audioSource.time = 0.6f;
        audioSource.Play();
    }
}
