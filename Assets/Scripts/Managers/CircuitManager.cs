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
    }

    public void Delete()
    {
        if (selectedObj == null) return;


        if (selectedObj.layer == LayerMask.NameToLayer("Body"))
        {
            var inputPortA = selectedObj.transform.Find("InputPortA").GetComponent<InputPort>();
            var inputPortB = selectedObj.transform.Find("InputPortB").GetComponent<InputPort>();
            var outputPort = selectedObj.transform.Find("OutputPort").GetComponent<OutputPort>();

            inputPortA.Disconnect(inputPortA.ConnectedOutput);
            inputPortB.Disconnect(inputPortB.ConnectedOutput);
            outputPort.DisconnectAll();
        
            Circuits.Remove(selectedObj);
            Object.Destroy(selectedObj);
        }

        if (selectedObj.layer == LayerMask.NameToLayer("Line"))
        {
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

    public void DeleteLine(GameObject go)
    {

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