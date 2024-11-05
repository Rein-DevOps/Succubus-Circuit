using System.Collections.Generic;
using UnityEngine;
public class OutCome : GateBase
{
    private List<bool> _answerList = new();
    private int _currentIndex = 0;

    protected override void Init()
    {
        base.Init();


        // 테스트용. 추후 지울 것
        _answerList.Add(false);
        _answerList.Add(true);
        _answerList.Add(false);
        _answerList.Add(true);

        SetAnswerList(_answerList);

        // answerList.Add(false);
        // answerList.Add(true);
        Debug.Log($"{gameObject.name} initialized with answer list: {string.Join(", ", _answerList)}");
    }

    public void SetAnswerList(List<bool> answerList)
    {
        _answerList = answerList;
        _currentIndex = 0;
        Debug.Log($"{gameObject.name} answer list set: {string.Join(", ", _answerList)}");
    }

    protected override bool Calculate()
    {
        // if (_calculated) return _result;

        if (_inputDataQueue.Count != inputPorts.Count)
        {
            Debug.LogError($"{gameObject.name} cannot calculate. Expected {inputPorts.Count} inputs, but has {_inputDataQueue.Count}.");
            TestEnd();
            return false;
        }

        bool received = _inputDataQueue.Dequeue();

        if (_currentIndex >= _answerList.Count)
        {
            Debug.LogError($"{gameObject.name} has received more inputs than expected.");
            _result = false;
            TestEnd();
            return false;
        }
        
        bool expected = _answerList[_currentIndex];

        if (received == expected)
        {
            _result = true;
        }
        else
        {
            _result = false;
        }
        
        _calculated = true;
        Debug.Log($"{gameObject.name} calculation completed. Result: {_result}");
        return _result;
    }

    public override void LocalTest()
    {
        Debug.Log($"{gameObject.name} starting outcome test.");
        Calculate();

        if (_result)
        {
            Debug.Log($"{gameObject.name} test result: Correct!");
        }
        else
        {
            Debug.Log($"{gameObject.name} test result: Incorrect!");
        }

        LocalTestEnd();
        // Debug.Log($"{gameObject.name} clearing input data list.");
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
