using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InputManager
{
    private GameObject _selectedObject;
    public GameObject SelectedObject
    {
        get { return _selectedObject; }
        private set { _selectedObject = value; }
    }

    private Define.CircuitComponent selectedComponent = Define.CircuitComponent.None;
    private Dictionary<GameObject, Action<Define.MouseEvent, Vector2>> interActions = new Dictionary<GameObject, Action<Define.MouseEvent, Vector2>>();
    private int circuitLayer = LayerMask.GetMask("Body", "Line", "InputPort", "OutputPort");

    public void Init()
    {

    }


    public void OnUpdate()
    {
        if (GameManager.currScene == Define.Scene.Stage)
        {
            
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log($"Mouse Position: {Input.mousePosition}");
            // Debug.Log($"Converted Position: {mousePos}");

            if (Input.GetMouseButtonDown(0))
            {
                if (SelectedObject != null)
                {
                    interActions[SelectedObject].Invoke(Define.MouseEvent.None, mousePos);
                }
                
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 20.0f);
                selectedComponent = GetSelectedComponentType(hit);
                
                Debug.Log($"Selected Component: {selectedComponent}");
                if (selectedComponent != Define.CircuitComponent.None)
                {
                    interActions[hit.collider.gameObject]?.Invoke(Define.MouseEvent.Click, mousePos);
                    SelectedObject = hit.collider.gameObject;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (selectedComponent == Define.CircuitComponent.None) return;

                interActions[SelectedObject]?.Invoke(Define.MouseEvent.Press, mousePos);
            }

            else if (Input.GetMouseButtonUp(0))
            {
                if (selectedComponent == Define.CircuitComponent.None) return;

                selectedComponent = Define.CircuitComponent.None;
                Debug.Log($"SelectedObject {SelectedObject}");
                interActions[SelectedObject]?.Invoke(Define.MouseEvent.Release, mousePos);
            }
        }
    }

    public void Subscribe(GameObject gate, Action<Define.MouseEvent, Vector2> listener)
    {
        if (!interActions.ContainsKey(gate))
        {
            interActions[gate] = listener;
        }
        else
        {
            interActions[gate] += listener;
        }
    }

    public void Unsubscribe(GameObject gate, Action<Define.MouseEvent, Vector2> listener)
    {
        if (interActions.ContainsKey(gate))
        {
            interActions[gate] -= listener;
        }
        if (interActions[gate] == null)
        {
            interActions.Remove(gate);
        }
    }

    public void Clear()
    {
        interActions.Clear();
    }

    public Define.CircuitComponent GetSelectedComponentType(RaycastHit2D hit)
    {
        if (hit.collider == null) return Define.CircuitComponent.None;
        if (IsPointerOverCircuitBody(hit)) return Define.CircuitComponent.Body;
        if (IsPointerOverCircuitLine(hit)) return Define.CircuitComponent.Line;
        if (IsPointerOverCircuitInputPort(hit)) return Define.CircuitComponent.InputPort;
        if (IsPointerOverCircuitOutputPort(hit)) return Define.CircuitComponent.OutputPort;
        return Define.CircuitComponent.None;
    }

    public bool IsPointerOverCircuitBody(RaycastHit2D hit)
    {
        if (hit.collider == null) return false;

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Body"))
            return true;
        
        return false;
    }

    public bool IsPointerOverCircuitLine(RaycastHit2D hit)
    {
        if (hit.collider == null) return false;

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Line"))
            return true;
        
        return false;
    }

    public bool IsPointerOverCircuitInputPort(RaycastHit2D hit)
    {
        if (hit.collider == null) return false;

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("InputPort"))
            return true;
        
        return false;
    }

    public bool IsPointerOverCircuitOutputPort(RaycastHit2D hit)
    {
        if (hit.collider == null) return false;

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("OutputPort"))
            return true;
        
        return false;
    }
}
