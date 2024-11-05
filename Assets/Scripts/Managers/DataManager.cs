using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class StageData
{
    public int stageNumber;
    public string gateType;
    public List<List<int>> inputs;
    public List<List<int>> outputs;
    public string infoText;
}

[Serializable]
public class StageDataList
{
    public List<StageData> stages;
}

public class DataManager
{
    private Dictionary<int, StageData> StageDict = new Dictionary<int, StageData>();

    public DataManager()
    {
        Init();
    }

    public void Init()
    {
        TextAsset textAsset = GameManager.Resource.Load<TextAsset>("Data/Stage");
        if (textAsset != null)
        {
            StageDataList data = JsonUtility.FromJson<StageDataList>(textAsset.text);

            foreach (StageData stage in data.stages)
            {
                StageDict.Add(stage.stageNumber, stage);
            }
            Debug.Log("Stage data loaded successfully.");
        }
        else
        {
            Debug.LogError("Stage.json not found in Resources/Data.");
        }
    }

    public StageData GetStageData(int stageNumber)
    {
        if (StageDict.ContainsKey(stageNumber))
        {
            return StageDict[stageNumber];
        }
        else
        {
            Debug.LogError($"Stage {stageNumber} data not found.");
            return null;
        }
    }

    public void LoadData()
    {

    }
}
