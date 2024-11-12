using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

public class CircuitManager
{
    public GameObject selectedObj;

    // public Dictionary<GateBase, List<GateBase>> gates = new();
    public Dictionary<GameObject, List<GameObject>> lines = new();
    public Dictionary<GameObject, List<GameObject>> inputports = new();
    public Dictionary<GameObject, List<GameObject>> outputports = new();
    public Dictionary<GameObject, List<GameObject>> connectports = new();

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

    public void InputPortSet(GameObject gate, GameObject inputport)
    {
        if (!inputports.ContainsKey(gate))
        {
            inputports[gate] = new List<GameObject>();
        }
        inputports[gate].Add(inputport);
    }

    public void OutputPortSet(GameObject gate, GameObject outputport)
    {
        if (!outputports.ContainsKey(gate))
        {
            outputports[gate] = new List<GameObject>();
        }
        outputports[gate].Add(outputport);
    }

    public void InputPortUnset(GameObject gate, GameObject inputport)
    {
        if (inputports.ContainsKey(gate))
        {
            inputports[gate].Remove(inputport);
        }
    }

    public void OutputPortUnset(GameObject gate, GameObject outputport)
    {
        if (outputports.ContainsKey(gate))
        {
            outputports[gate].Remove(outputport);
        }
    }

    public void LineConnect(GameObject firstPort, GameObject secondPort, GameObject lineObject)
    {
        if (!lines.ContainsKey(firstPort))
        {
            lines[firstPort] = new List<GameObject>();
        }
        lines[firstPort].Add(lineObject);

        if (!lines.ContainsKey(secondPort))
        {
            lines[secondPort] = new List<GameObject>();
        }
        lines[secondPort].Add(lineObject);
    }

    public void LineDisconnect(GameObject port, GameObject lineObject)
    {
        if (lines.ContainsKey(port) && lines[port].Contains(lineObject))
        {
            lines[port].Remove(lineObject);
        }

        foreach(var otherPort in connectports[port])
        {
            if (lines.ContainsKey(otherPort) && lines[otherPort].Contains(lineObject))
            {
                lines[port].Remove(lineObject);
            }
        }
    }

    public void PortConnect(GameObject firstPort, GameObject secondPort)
    {
        if (!connectports.ContainsKey(firstPort))
        {
            connectports[firstPort] = new List<GameObject>();
        }
        connectports[firstPort].Add(secondPort);

        if (!connectports.ContainsKey(secondPort))
        {
            connectports[secondPort] = new List<GameObject>();
        }
        connectports[secondPort].Add(firstPort);
    }

    public void PortDisconnect(GameObject firstPort, GameObject secondPort)
    {
        
    }



    public void GateConnect(GateBase firstGate, GateBase secondGate)
    {

    }

    public void Init()
    {
        Clear();
        var root = GateRoot;
    }

    public void InitSetting(int inputNum, int outputNum)
    {

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


    // InstantiateSwitches(int stageNumber)
    // {
    //     string path = "Prefabs/Gates/Switch";
    //     Debug.Log("Switches Created");

    //     float yPosition = -4.0f;
    //     float xOffset = 1.0f; // x 간격의 절반값 (간격이 2이므로)

    //     int switchNumber = 1;

    //     for (int i = 0; i < switchNumber; i++)
    //     {
    //         // 중앙 정렬을 위해 x 위치 계산
    //         float xPosition = (i - (switchNumber - 1) / 2.0f) * 2.0f * xOffset;

    //         // 생성하고 위치 설정
    //         GameObject go = GameManager.Resource.Instantiate(path, GateRoot.transform);
    //         go.transform.position = new Vector3(xPosition, yPosition, 0);
    //     }
    // }

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

    public void Delete()
    {
        if (selectedObj == null){
            Debug.Log("Selected Object is NULL!");
            return;
        } 


        if (selectedObj.layer == LayerMask.NameToLayer("Body"))
        {
            Debug.Log("Body Delete Called !");
            // DeleteCircuit(selectedObj);
            return;
        }

        if (selectedObj.layer == LayerMask.NameToLayer("Line"))
        {
            Debug.Log("Line Delete Called !");
            DeleteLine(selectedObj);
            return;
        }   
    }

    // public void DeleteCircuit(GameObject go)
    // {
    //     if (go == null) return;


    //     Debug.Log("Delete Circuit Called! ");
    //     var inputPortA = go.transform.parent.Find("InputPortA").GetComponent<InputPort>();
    //     var inputPortB = go.transform.parent.Find("InputPortB").GetComponent<InputPort>();
    //     var outputPort = go.transform.parent.Find("OutputPort").GetComponent<OutputPort>();

    //     if (inputPortA != null)
    //         inputPortA.Disconnect(inputPortA.ConnectedOutput);
    //     if (inputPortB != null)
    //         inputPortB.Disconnect(inputPortB.ConnectedOutput);
    //     if (outputPort != null)
    //         outputPort.DisconnectAll();
        
    //     if(Circuits.Contains(go.transform.parent.gameObject))
    //         Circuits.Remove(go.transform.parent.gameObject);
    //     Object.Destroy(go.transform.parent.gameObject);
    // }

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

        // // 모든 회로를 순회하며 해당 LineRenderer를 가진 OutputPort 찾기
        // foreach (var circuit in Circuits)
        // {
        //     var outputPort = circuit.transform.Find("OutputPort")?.GetComponent<OutputPort>();
        //     if (outputPort != null && outputPort.HasLine(lineRenderer))
        //     {
        //         foundOutputPort = outputPort;
        //         break;
        //     }
        // }

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

    }

    public void Test()
    {
        // Debug.LogWarning($"Test Start. {Switches[0].GetComponent<Switch>()._dataSequence.Count}");
        // for(int i = 0; i < Switches[0].GetComponent<Switch>()._dataSequence.Count; i++)
        // {
        //     foreach(var swt in Switches)
        //     {
        //         swt.GetComponent<Switch>().LocalTest();
        //     }
        // }

        // Clear();
    }

    public bool TestEnd(OutCome outCome, int expected, int correct)
    {
        Debug.LogWarning($"Expected: {expected} Correct: {correct}");
        IsCorrect = (expected == correct);
        
        return IsCorrect;
    }
}