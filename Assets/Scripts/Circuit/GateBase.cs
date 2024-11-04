using System.Collections.Generic;
using UnityEngine;

public abstract class GateBase : MonoBehaviour
{
    protected bool _calculated = false;
    protected bool _result = false;
    protected List<InputPort> inputPorts;
    protected OutputPort outputPort;

    void Start()
    {

    }

    

    protected abstract bool Calculate();
}
