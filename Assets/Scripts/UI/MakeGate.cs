using UnityEngine;

public class MakeGate : UIBase
{
    enum Buttons
    {
        MakeButton,
        DeleteButton,
        TestButton
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Init()
    {
        GameManager.UI.SetCanvas(gameObject, false);
    }
}
