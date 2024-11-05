using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InputManager
{
    public Action<Define.MouseEvent> MouseAction = null;

    public void OnUpdate()
    {
        if (GameManager.gameState == GameManager.GameState.Circuit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Perform a Raycast to check if pointer is over UI
                if (IsPointerOverUIElement())
                {
                    // Pointer is over UI, handle UI interaction
                    MouseAction?.Invoke(Define.MouseEvent.UISelect);
                }
                else
                {
                    // Pointer is not over UI, proceed with selection
                    MouseAction?.Invoke(Define.MouseEvent.Select);
                }
            }
        }
    }

    private bool IsPointerOverUIElement()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Perform a 2D raycast using the UI layer mask
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 20.0f, LayerMask.NameToLayer("Body") | LayerMask.NameToLayer("Line"));

        // Returns true if a collider in the specified layer was hit
        return hit.collider == null;
    }
}
