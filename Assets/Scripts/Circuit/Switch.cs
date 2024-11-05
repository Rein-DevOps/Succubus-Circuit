using System.Collections.Generic;
using UnityEngine;

public class Switch : GateBase
{
    public List<bool> _dataSequence = new List<bool>();
    private int _currentIndex = 0;

    protected override void Init()
    {
        base.Init();
        _calculated = true; // 초기 상태 설정

        // Local로 데이터 정해줌. 추후 구현 후 삭제할 것



        GameManager.Circuit.SetSwitch(gameObject);
        SetDataSequence(_dataSequence);
        Debug.Log($"{gameObject.name} initialized with {_dataSequence.Count} data points.");
    }

    public void SetDataSequence(List<bool> dataSequence)
    {
        _dataSequence = dataSequence;
        _currentIndex = 0;
        Debug.Log($"{gameObject.name} data sequence set: {string.Join(", ", _dataSequence)}");
    }

    protected override bool Calculate()
    {
        if (_currentIndex >= _dataSequence.Count)
        {
            Debug.Log($"{gameObject.name} has no more data to send.");
            _calculated = false;
            return false;
        }

        _result = _dataSequence[_currentIndex];
        Debug.Log($"{gameObject.name} calculated result for index {_currentIndex}: {_result}");
        return _result;
    }

    public override void LocalTest()
    {
        if (_currentIndex >= _dataSequence.Count)
        {
            Debug.Log($"{gameObject.name} has completed all data sequences.");
            LocalTestEnd();
            return;
        }

        Debug.Log($"{gameObject.name} Test() called for index {_currentIndex}");
        Calculate();

        foreach (var output in outputPort.ConnectedInputs)
        {
            GateBase connectedGate = output.transform.parent.GetComponent<GateBase>();
            Debug.Log($"{gameObject.name} sending data {_result} to {connectedGate.gameObject.name}");
            connectedGate.SetData(_result);
        }
        LocalTestEnd();
        if (_currentIndex == _dataSequence.Count)
        {
            TestEnd();
        }
    }

    public override void LocalTestEnd()
    {
        _currentIndex++;
    }

    public override void TestEnd()
    {
        base.TestEnd();
        _currentIndex = 0;
    }


}
