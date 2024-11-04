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

    public void InstantiateSwitches(string data)
    {

    }

    public void InstantiateOutcome(string data)
    {
        
    }

    public void Select(GameObject go)
    {
        selectedObj = go;
        Debug.Log("Gate Selected !");
    }

    public void Delete()
    {
        if (selectedObj == null){
            Debug.Log("Selected Object is NULL!");
            return;
        } 


        if (selectedObj.layer == LayerMask.NameToLayer("Body"))
        {
            DeleteCircuit(selectedObj);
            return;
        }

        if (selectedObj.layer == LayerMask.NameToLayer("Line"))
        {
            DeleteLine(selectedObj);
            return;
        }   
    }

    public void DeleteCircuit(GameObject go)
    {
        if (go == null || !Circuits.Contains(go)) return;

        var inputPortA = go.transform.Find("InputPortA").GetComponent<InputPort>();
        var inputPortB = go.transform.Find("InputPortB").GetComponent<InputPort>();
        var outputPort = go.transform.Find("OutputPort").GetComponent<OutputPort>();

        if (inputPortA != null)
            inputPortA.Disconnect(inputPortA.ConnectedOutput);
        if (inputPortB != null)
            inputPortB.Disconnect(inputPortB.ConnectedOutput);
        if (outputPort != null)
            outputPort.DisconnectAll();
        
        Circuits.Remove(go);
        Object.Destroy(go);
    }

    public void DeleteLine(GameObject lineObj)
    {
        if (lineObj == null || lineObj.layer != LayerMask.NameToLayer("Line")) return;

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

    public void SwitchOn()
    {

    }
}