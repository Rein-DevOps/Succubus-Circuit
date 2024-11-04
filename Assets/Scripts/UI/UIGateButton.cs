using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGateButton : UIScene
{
    
    [SerializeField]
    TextMeshProUGUI _text;

    enum Buttons
    {
        MakeButton,
        DeleteButton,
        TestButton,
        StageInfoButton,
    }
    enum Texts
    {
        MakeText,
        DeleteText,
        TestText,
        StageInfoText,
    }

    enum Images
    {
        MakeIcon,
        DeleteIcon,
        TestIcon,
        StageInfoIcon
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
        Bind<Image>(typeof(Images));

        // GetText((int) Texts.ScoreText).text = "Bind Test";
        
        //GetButton((int) Buttons.MakeButton).onClick.AddListener(() => OnGateMake(null));

        GameObject makeObject = GetButton((int) Buttons.MakeButton).gameObject;
        GameObject DeleteObject = GetButton((int) Buttons.DeleteButton).gameObject;
        GameObject TestObject = GetButton((int) Buttons.TestButton).gameObject;

        BindEvent(makeObject, OnGateMake, Define.UIEvent.Click);
        BindEvent(DeleteObject, OnObjectDelete, Define.UIEvent.Click);
        BindEvent(TestObject, OnTest, Define.UIEvent.Click);
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

    }
}
