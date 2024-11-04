using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Signal
{
    public bool Value { get; private set; }

    public Signal(bool initialValue = false)
    {
        Value = initialValue;
    }

    public void SetValue(bool newValue)
    {
        Value = newValue;
    }
}
