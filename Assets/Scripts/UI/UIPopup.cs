using UnityEngine;

public class UIPopup : UIBase
{
    public override void Init()
    {
        GameManager.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        GameManager.UI.ClosePopupUI(this);
    }
}
