using UnityEngine;

public class UIScene : UIBase
{
    public override void Init()
    {
        GameManager.UI.SetCanvas(gameObject, false);
    }
}
