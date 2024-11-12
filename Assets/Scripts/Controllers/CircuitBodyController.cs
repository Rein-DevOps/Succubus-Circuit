using System.Collections.Generic;
using UnityEngine;


public class CircuitBodyController : MonoBehaviour, ISelectable, IMouseInteractable
{
    private bool IsSelected = false;
    private Vector2 offset = new Vector2();
    private Transform selectCircle = null;
    private List<GameObject> connectedLines = new();
    private GameObject parentObject = null;

    void Awake()
    {
        if (transform.parent == null)
        {
            Debug.LogWarning("Parent object is null.");
            Debug.Log($"{gameObject}");
            return;
        }

        parentObject = transform.parent.gameObject;

        if (selectCircle == null)
        {
            selectCircle = transform.parent.Find("SelectCircle");

            if (selectCircle == null)
            {
                Debug.LogWarning("SelectCircle object not found in parent.");
            }
        }
    }

    private void OnInteraction(Define.MouseEvent mouseEvent, Vector2 mousePos)
    {
        switch (mouseEvent)
        {
            case Define.MouseEvent.None:
                OnDeselect();
                break;
            case Define.MouseEvent.Click:
                OnSelect();
                OnClick(mousePos);
                break;
            case Define.MouseEvent.Press:
                OnPress(mousePos);
                break;
            case Define.MouseEvent.Release:
                OnRelease(mousePos);
                Reset();
                break;
        }
    }

    public void OnClick(Vector2 mousePosition)
    {
        offset = (Vector2)transform.parent.position - mousePosition;
        // CircuitManager에 요청해서 현재 선택된 Gate에 연결되어 있는 Line들의 정보를 받아 올 것
        // GameManager.Circuit.
    }

    public void OnPress(Vector2 mousePosition)
    {
        if (IsSelected)
        {
            transform.parent.position = mousePosition + offset;
        }
    }

    public void OnRelease(Vector2 mousePosition)
    {
        transform.parent.position = mousePosition + offset;
        offset = Vector2.zero;
    }

    void OnEnable()
    {
        if (GameManager.currScene == Define.Scene.Stage)
        {
            GameManager.Input.Subscribe(gameObject, OnInteraction);
        }
    }

    void OnDisable()
    {
        if (GameManager.currScene == Define.Scene.Stage)
        {
            GameManager.Input.Unsubscribe(gameObject, OnInteraction);
        }
        OnDeselect();
        Reset();
    }

    public void OnSelect()
    {
        IsSelected = true;
        
        if (IsSelected)
        {
            if (selectCircle != null)
            {
                selectCircle.gameObject.SetActive(true);
            }
        }
    }

    public void OnDeselect()
    {
        IsSelected = false;

        if (GameManager.Input.SelectedObject != null && GameManager.Input.SelectedObject == gameObject)
        {
            selectCircle.gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        connectedLines.Clear();
    }
}
