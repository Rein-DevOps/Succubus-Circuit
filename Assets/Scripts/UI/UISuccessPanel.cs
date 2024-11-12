using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISuccessPanel : UIPopup
{
    // enum Buttons
    // {
    //     HomeButton,
    //     NextButton,
    //     ReplayButton,
    // }
    // enum Texts
    // {
    //     SuccessText,
    // }

    // // int _score = 0;

    // void Start()
    // {
    //     Init();
    // }

    // public void OnButtonClicked(PointerEventData data)
    // {
    //     Debug.Log("Button Click!");
    // }

    // public override void Init()
    // {
    //     base.Init();

    //     Bind<Button>(typeof(Buttons));
    //     Bind<TextMeshProUGUI>(typeof(Texts));

    //     // GetText((int) Texts.ScoreText).text = "Bind Test";
        
    //     //GetButton((int) Buttons.MakeButton).onClick.AddListener(() => OnGateMake(null));

    //     GameObject homeObject = GetButton((int) Buttons.HomeButton).gameObject;
    //     GameObject nextObject = GetButton((int) Buttons.NextButton).gameObject;
    //     GameObject replayObject = GetButton((int) Buttons.ReplayButton).gameObject;

    //     BindEvent(homeObject, OnHome, Define.UIEvent.Click);
    //     BindEvent(nextObject, OnNext, Define.UIEvent.Click);
    //     BindEvent(replayObject, OnReplay, Define.UIEvent.Click);
    // }

    // public void OnHome(PointerEventData data)
    // {
    //     Debug.Log("Go to Menu Called");
    //     GameManager.SceneChange(Define.Scene.Menu);
    // }
    // public void OnNext(PointerEventData data)
    // {
    //     Debug.Log("Gate Make Button Called");
    //     int currScene = SceneManager.GetActiveScene().buildIndex;

    //     if ((int) Define.Scene.LastScene >= currScene + 1)
    //     {
    //         GameManager.SceneChange((Define.Scene) currScene + 1, currScene + 1);
    //     }
    //     else
    //     {
    //         GameManager.SceneChange(Define.Scene.Menu);
    //     }
    // }
    // public void OnReplay(PointerEventData data)
    // {
    //     Debug.Log("Gate Make Button Called");
    //     int currScene = SceneManager.GetActiveScene().buildIndex;

    //     Debug.LogError($"CurrScene: {currScene}");
    //     GameManager.SceneChange((Define.Scene) currScene, currScene);
    // }
}
