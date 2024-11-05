using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public GameObject stageSelectPanel;
    public Button startButton;
    public Button exitButton;


    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButonClicked);
    }

    public void OnStartButtonClicked()
    {
        Debug.Log("Game Start!");
        

        gameObject.SetActive(false);
        stageSelectPanel.SetActive(true);
    }

    public void OnExitButonClicked()
    {
        Application.Quit();
    }
}
