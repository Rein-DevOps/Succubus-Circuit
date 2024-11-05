using System.Collections.Generic;
using UnityEngine;

public class CircuitManager
{
    public GameObject selectedObj;
    private List<GameObject> Circuits = new();
    public GameObject outcome;
    public List<GameObject> Switches = new();

    public GameObject GateRoot
    {
        get
        {
            GameObject gateRoot = GameObject.Find("@GateRoot");

            if (gateRoot == null)
            {
                gateRoot = new GameObject { name = "@GateRoot" };
            }
            return gateRoot;
        }
    }

    public void Init()
    {
        Clear();

    }

    public void InstantiateCircuit(string path = "Gates/NandGate")
    {
        Debug.Log($"{path} Circuit Maked");
        GameObject go = GameManager.Resource.Instantiate(path, GateRoot.transform);
        Circuits.Add(go);
    }

    public void ConfigureStage(StageData stageData)
    {
        // 스위치 생성 및 데이터 설정
        for (int i = 0; i < stageData.inputs.Count; i++)
        {
            GameObject switchObj = Switches[i];
            Switch swt = switchObj.GetComponent<Switch>();
            List<bool> inputData = new List<bool>();
            foreach (int val in stageData.inputs[i])
            {
                inputData.Add(val == 1);
            }
            swt.SetDataSequence(inputData);
        }

        // 아웃컴에 정답 설정
        GameObject outcomeObj = InstantiateOutcome();
        Outcome outcome = outcomeObj.GetComponent<Outcome>();
        List<bool> outputData = new List<bool>();
        foreach (int val in stageData.outputs[0])
        {
            outputData.Add(val == 1);
        }
        outcome.SetAnswerList(outputData);

        // 필요한 게이트 생성
        InstantiateGate(stageData.gateType);

        // 회로 연결은 플레이어가 직접 구성하므로 여기서는 생성만 합니다.
    }


    public void InstantiateSwitches(int switchNumber)
    {
        string path = "Prefabs/Gates/Switch";
        Debug.Log("Switches Created");

        float yPosition = -4.0f;
        float xOffset = 1.0f; // x 간격의 절반값 (간격이 2이므로)

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

    public void InstantiateOutcome(int num = 1)
    {
        string path = "Prefabs/Gates/OutCome";
        Debug.Log("Outcome Maked");

        // 기준 위치
        float yPosition = 4.0f;
        float xOffset = 1.0f; // x 간격의 절반값

        for (int i = 0; i < num; i++)
        {
            float xPosition = (i - (num - 1) / 2.0f) * 2.0f * xOffset;

            GameObject go = GameManager.Resource.Instantiate(path, GateRoot.transform);
            go.transform.position = new Vector3(xPosition, yPosition, 0);
            Circuits.Add(go);
        }
}

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
        foreach(var go in Circuits)
        {
            DeleteCircuit(go);
        }
        Circuits.Clear();
    }

    public void Test()
    {
        foreach (var swt in Switches)
        {
            //swt.GetComponent<Switch>().SetDataSequence
        }
    }
}