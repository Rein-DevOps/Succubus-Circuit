using System.Collections.Generic;
using UnityEngine;

public abstract class GateBase : MonoBehaviour
{
    protected bool _calculated = false;
    protected bool _result = false;
    protected Queue<bool> _inputDataQueue = new();
    protected List<GameObject> connectedObjects = new();

    protected List<InputPort> inputPorts = new();
    protected OutputPort outputPort;

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        inputPorts.AddRange(gameObject.GetComponentsInChildren<InputPort>());
        outputPort = gameObject.GetComponentInChildren<OutputPort>();
        Debug.Log($"{gameObject.name} initialized with {inputPorts.Count} input ports and 1 output port.");
    }

    protected abstract bool Calculate();

    public virtual void LocalTest()
    {
        Debug.Log($"{gameObject.name} starting test.");
        Calculate();

        if (!_calculated)
        {
            LocalTestEnd();
            return;
        }
        
        Debug.Log($"{gameObject.name} calculation result: {_result}");

        foreach (var output in outputPort.ConnectedInputs)
        {
            GateBase connectedGate = output.transform.parent.GetComponent<GateBase>();
            Debug.Log($"{gameObject.name} sending data to {connectedGate.gameObject.name}: {_result}");
            connectedGate.SetData(_result);
            connectedObjects.Add(connectedGate.gameObject);
        }

        /*
        HashSet<GameObject> uniqueConnected = new(connectedObjects);

        foreach (var connected in uniqueConnected)
        {
            Debug.Log($"{gameObject.name} triggering test on connected gate: {connected.name}");
            connected.GetComponent<GateBase>().Test();
        }
        */

        LocalTestEnd();
    }


    public virtual bool GetResult()
    {
        return _result;
    }

    public virtual void SetData(bool data)
    {
        _inputDataQueue.Enqueue(data);
        Debug.Log($"{gameObject.name} received data: {data}");

        if (_inputDataQueue.Count == inputPorts.Count)
        {
            Debug.Log($"{gameObject.name} has received all inputs. Starting test.");
            LocalTest();
        }
    }

    public virtual void SendData(GateBase gateBase, bool data)
    {
        gateBase.SetData(data);
    }

    public virtual void LocalTestEnd()
    {
        Debug.Log($"{gameObject.name} ending local test. Clearing calculated state.");
        _calculated = false;
        _inputDataQueue.Clear();
        connectedObjects.Clear();
    }

    public virtual void TestEnd()
    {
        Debug.Log($"{gameObject.name} ending local test. Clearing calculated state.");
        _calculated = false;
        _inputDataQueue.Clear();
        connectedObjects.Clear();
    }
}
