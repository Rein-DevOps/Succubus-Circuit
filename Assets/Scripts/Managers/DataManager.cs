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


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Stage>StageDict { get; private set; } = new Dictionary<int, Stage>();

    public void Init()
    {
        TextAsset textAsset = GameManager.Resource.Load<TextAsset>($"Data/Stage");
        StageData data = JsonUtility.FromJson<StageData>(textAsset.text);

        foreach(Stage stage in data.stages)
        {
            StageDict.Add(stage.stageNumber, stage);
        }
    }

    public void LoadData()
    {
        
    }
}