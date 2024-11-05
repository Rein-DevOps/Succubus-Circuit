using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStageSelect : MonoBehaviour
{
    enum Stage
    {
        One,
        Two,
        Three,
        Four
    }

    public GameObject gameMainPanel;
    public List<Button> stageButtons;
    public Button returnButton;

    void Start()
    {
        for (int i = 0; i < stageButtons.Count; i++)
        {
            int stageNum = i; // 로컬 변수를 만들어 현재 인덱스를 캡처합니다.
            stageButtons[i].onClick.AddListener(() => OnStageButtonClicked(stageNum));
        }

        returnButton.onClick.AddListener(OnReturnButtonClicked);
    }

    public void OnReturnButtonClicked()
    {
        Debug.Log("Game Return!");
        gameObject.SetActive(false);
        gameMainPanel.SetActive(true);
    }

    public void OnStageButtonClicked(int stageNum)
    {
        Debug.Log($"Stage {stageNum + 1} selected");
        GameManager.SceneChange(Define.Scene.Game, stageNum);
    }
}
