using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager
{
    public int stageNum = 0;
    private DataManager _dataManager = new DataManager();

    public void LoadStage(int stageNumber)
    {
        stageNum = stageNumber;
        // 스테이지 데이터를 로드
        StageData stageData = GameManager.Data.GetStageData(stageNum);

        if (stageData != null)
        {
            // 회로 초기화
            GameManager.Circuit.Init();

            // 스위치 및 아웃컴 생성
            GameManager.Circuit.InstantiateSwitches(stageData.inputs.Count);
            GameManager.Circuit.InstantiateOutcome(1); // Outcome은 1개로 가정

            // 스테이지 정보를 CircuitManager에 전달하여 회로 구성
            GameManager.Circuit.ConfigureStage(stageData);

            // 게임 씬 로드
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError($"Stage {stageNum} data not found.");
        }
    }

    public void Clear()
    {

    }

    public void SavePlayerArchieve()
    {
        // 현재 플레이어가 클리어한 스테이지가 기존 PlayerPrefs에서 저장된 스테이지보다 높을 경우 저장
    }

    public void LoadPlayerAchieve()
    {

    }
}
