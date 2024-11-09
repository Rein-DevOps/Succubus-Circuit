using System.Collections.Generic;
using UnityEngine;

public class CircuitManager
{
    public GameObject selectedObj;
    private List<GameObject> Circuits = new();
    public List<GameObject> Switches = new();
    public List<GameObject> outcome = new();
    public bool IsCorrect = false;
    

    private GameObject _gateRoot;
    
    public GameObject GateRoot
    {
        get
        {
            if (_gateRoot == null)
            {
                _gateRoot = GameObject.Find("@GateRoot");
                if (_gateRoot == null)
                {
                    _gateRoot = new GameObject { name = "@GateRoot" };
                }
            }
            return _gateRoot;
        }
    }

    public void Init()
    {
        Clear();
        var root = GateRoot;
    }

    public void SetSwitch(GameObject swt)
    {
        Switches.Add(swt);
    }

    public void InstantiateCircuit(string path = "Gates/NandGate")
    {
        Debug.Log($"{path} Circuit Maked");
        GameObject go = GameManager.Resource.Instantiate(path, GateRoot.transform);
        Circuits.Add(go);
    }

    // public void ConfigureStage(StageData stageData)
    // {
    //     // 스위치 생성 및 데이터 설정

    //     InstantiateSwitches(stageData.inputs.Count);

    //     for (int i = 0; i < stageData.inputs.Count; i++)
    //     {
    //         GameObject switchObj = Switches[i];
    //         Switch swt = switchObj.GetComponent<Switch>();
    //         List<bool> inputData = new List<bool>();
    //         foreach (int val in stageData.inputs[i])
    //         {
    //             inputData.Add(val == 1);
    //         }
    //         swt.SetDataSequence(inputData);
    //     }

    //     // 아웃컴에 정답 설정
    //     GameObject outcomeObj = InstantiateOutcome(stageData.outputs.Count);
    //     OutCome outcome = outcomeObj.GetComponent<OutCome>();
    //     List<bool> outputData = new List<bool>();
    //     foreach (int val in stageData.outputs[0])
    //     {
    //         outputData.Add(val == 1);
    //     }
    //     outcome.SetAnswerList(outputData);

    //     // 회로 연결은 플레이어가 직접 구성하므로 여기서는 생성만 합니다.
    // }


    public void InstantiateSwitches(int stageNumber)
    {
        string path = "Prefabs/Gates/Switch";
        Debug.Log("Switches Created");

        float yPosition = -4.0f;
        float xOffset = 1.0f; // x 간격의 절반값 (간격이 2이므로)

        int switchNumber = 1;


        // switch (stageNumber)
        // {
        //     case 1:
        //         switchNumber = 1;
        //     break;

        //     case 2:
        //         switchNumber = 2;
        //     break;

        //     case 3:
        //         switchNumber = 2;
        //     break;

        //     case 4:
        //         switchNumber = 2;
        //     break;
        // }

        for (int i = 0; i < switchNumber; i++)
        {
            // 중앙 정렬을 위해 x 위치 계산
            float xPosition = (i - (switchNumber - 1) / 2.0f) * 2.0f * xOffset;

            // 생성하고 위치 설정
            GameObject go = GameManager.Resource.Instantiate(path, GateRoot.transform);
            go.transform.position = new Vector3(xPosition, yPosition, 0);
            Switches.Add(go);
            Circuits.Add(go);
        }
    }

    // public GameObject InstantiateOutcome(int num = 1)
    // {
    //     string path = "Prefabs/Gates/OutCome";
    //     Debug.Log("Outcome Maked");

    //     // 기준 위치
    //     float yPosition = 4.0f;
    //     float xOffset = 1.0f; // x 간격의 절반값

    //     // 우선 하나만 있다고 보고

    //     // for (int i = 0; i < num; i++)
    //     // {
    //     //     float xPosition = (i - (num - 1) / 2.0f) * 2.0f * xOffset;

    //     //     GameObject go = GameManager.Resource.Instantiate(path, GateRoot.transform);
    //     //     go.transform.position = new Vector3(xPosition, yPosition, 0);
    //     //     Circuits.Add(go);
    //     // }

    //     float xPosition = 0;

    //     GameObject go = GameManager.Resource.Instantiate(path, GateRoot.transform);
    //     go.transform.position = new Vector3(xPosition, yPosition, 0);
    //     Circuits.Add(go);
    //     return go;
    // }

    public void Select(GameObject go)
    {
        selectedObj = go;

        if (selectedObj.layer == LayerMask.NameToLayer("Body"))
        {
            Debug.Log("Gate Selected !");
        }
        if (selectedObj.layer == LayerMask.NameToLayer("Line"))
        {
            Debug.Log("Line Selected !");
        }
    }

    public void Delete()
    {
        if (selectedObj == null){
            Debug.Log("Selected Object is NULL!");
            return;
        } 


        if (selectedObj.layer == LayerMask.NameToLayer("Body"))
        {
            Debug.Log("Body Delete Called !");
            DeleteCircuit(selectedObj);
            return;
        }

        if (selectedObj.layer == LayerMask.NameToLayer("Line"))
        {
            Debug.Log("Line Delete Called !");
            DeleteLine(selectedObj);
            return;
        }   
    }

    public void DeleteCircuit(GameObject go)
    {
        if (go == null) return;


        Debug.Log("Delete Circuit Called! ");
        var inputPortA = go.transform.parent.Find("InputPortA").GetComponent<InputPort>();
        var inputPortB = go.transform.parent.Find("InputPortB").GetComponent<InputPort>();
        var outputPort = go.transform.parent.Find("OutputPort").GetComponent<OutputPort>();

        if (inputPortA != null)
            inputPortA.Disconnect(inputPortA.ConnectedOutput);
        if (inputPortB != null)
            inputPortB.Disconnect(inputPortB.ConnectedOutput);
        if (outputPort != null)
            outputPort.DisconnectAll();
        
        if(Circuits.Contains(go.transform.parent.gameObject))
            Circuits.Remove(go.transform.parent.gameObject);
        Object.Destroy(go.transform.parent.gameObject);
    }

    public void DeleteLine(GameObject lineObj)
    {
        if (lineObj == null || lineObj.layer != LayerMask.NameToLayer("Line")) return;

        Debug.Log("Delete Line Called! ");
        // LineRenderer 컴포넌트 가져오기
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer not found on lineObj.");
            return;
        }

        OutputPort foundOutputPort = null;

        // 모든 회로를 순회하며 해당 LineRenderer를 가진 OutputPort 찾기
        foreach (var circuit in Circuits)
        {
            var outputPort = circuit.transform.Find("OutputPort")?.GetComponent<OutputPort>();
            if (outputPort != null && outputPort.HasLine(lineRenderer))
            {
                foundOutputPort = outputPort;
                break;
            }
        }

        if (foundOutputPort != null)
        {
            // 해당 OutputPort에서 LineRenderer와 연결된 InputPort 해제
            foundOutputPort.DisconnectLine(lineRenderer);
            // Line 오브젝트 삭제
            Object.Destroy(lineObj);
        }
        else
        {
            Debug.LogError("No OutputPort found with the given LineRenderer.");
        }
    }

    public void DeleteSwitches()
    {

    }

    public void DeleteOutcome()
    {

    }

    public void Clear()
    {
        // foreach(var go in Circuits)
        // {
        //     DeleteCircuit(go);
        // }
        Switches.Clear();
        Circuits.Clear();
    }

    public void Test()
    {
        Debug.LogWarning($"Test Start. {Switches[0].GetComponent<Switch>()._dataSequence.Count}");
        for(int i = 0; i < Switches[0].GetComponent<Switch>()._dataSequence.Count; i++)
        {
            foreach(var swt in Switches)
            {
                swt.GetComponent<Switch>().LocalTest();
            }
        }

        // Clear();
    }

    public bool TestEnd(OutCome outCome, int expected, int correct)
    {
        Debug.LogWarning($"Expected: {expected} Correct: {correct}");
        IsCorrect = (expected == correct);
        
        return IsCorrect;
    }
}