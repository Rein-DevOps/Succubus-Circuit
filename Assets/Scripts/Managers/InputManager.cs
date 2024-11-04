using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action<Define.MouseEvent> MouseAction = null;

    public void OnUpdate()
    {
        if (GameManager.gameState == GameManager.GameState.Circuit)
        {
            if (!EventSystem.current.IsPointerOverGameObject() && MouseAction != null)
            {
                Debug.Log("PointerOverObject");
                if (Input.GetMouseButtonDown(0))
                {
                    MouseAction.Invoke(Define.MouseEvent.Select);
                }
            }
        }

        if (GameManager.gameState == GameManager.GameState.Dialogue)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Click);
            }
        }
    }
}

