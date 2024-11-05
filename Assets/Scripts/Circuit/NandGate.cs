using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NandGate : GateBase
{
    protected override bool Calculate()
    {
        // if (_calculated) return _result;

        foreach (var inputPort in inputPorts)
        {
            if (inputPort.ConnectedOutput == null)
            {
                Debug.LogError($"{gameObject.name} cannot calculate. InputPort {inputPort.gameObject.name} is not connected.");
                LocalTestEnd();
                return false;
            }
        }

        if (_inputDataQueue.Count != inputPorts.Count)
        {
            Debug.LogError($"{gameObject.name} cannot calculate. Expected {inputPorts.Count} inputs, but has {_inputDataQueue.Count}.");
            _calculated = false;
            return false;
        }

        bool input1 = _inputDataQueue.Dequeue();
        bool input2 = _inputDataQueue.Dequeue();

        // NAND 연산 수행
        _result = !(input1 && input2);
        _calculated = true;

        Debug.Log($"{gameObject.name} NAND calculation result: {_result}");
        return _result;
    }
}
