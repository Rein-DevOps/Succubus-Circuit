using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIManager
{
    int _order = 10;
    UIScene _sceneUI = null;
    Stack<UIPopup> _popupStack = new Stack<UIPopup>();

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UIRoot");

            if (root == null)
            {
                root = new GameObject { name = "@UIRoot" };
            }
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }

        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);
        
        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);
        go.transform.SetParent(Root.transform);
        
        return popup;
    }

    public void ClosePopupUI(UIPopup popup)
    {
        if (_popupStack.Count == 0) return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }
        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0) return;

        UIPopup popup = _popupStack.Pop();
        GameManager.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void Clear()
    {
        // CloseAllPopupUI();
        _sceneUI = null;
    }
}
