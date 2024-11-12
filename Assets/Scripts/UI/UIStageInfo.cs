using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStageInfo : UIPopup
{
    
    [SerializeField]
    TextMeshProUGUI _text;

    enum Buttons
    {
        ExitButton
    }
    enum Texts
    {
        Info,
    }
    
    enum GameObjects
    {
        
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
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        // GetText((int) Texts.ScoreText).text = "Bind Test";
        
        //GetButton((int) Buttons.MakeButton).onClick.AddListener(() => OnGateMake(null));

        GameObject exitObject = GetButton((int) Buttons.ExitButton).gameObject;

        BindEvent(exitObject, OnGateMake, Define.UIEvent.Click);
    }

    public void OnGateMake(PointerEventData data)
    {
        Debug.Log("Gate Make Button Called");
        // GameManager.Circuit.InstantiateCircuit();
    }
}
