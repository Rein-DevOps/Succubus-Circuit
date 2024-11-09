using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class CircuitBodyController : MonoBehaviour, ISelectable, IMouseInteractable
{
    private bool IsSelected = false;
    private Vector2 offset = new Vector2();
    private Transform selectCircle;

    void Awake()
    {
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
                break;
        }
    }

    public void OnClick(Vector2 mousePosition)
    {
        offset = (Vector2)transform.position - mousePosition;
    }

    public void OnPress(Vector2 mousePosition)
    {
        if (IsSelected)
        {
            transform.position = mousePosition + offset;
        }
    }

    public void OnRelease(Vector2 mousePosition)
    {
        transform.position = mousePosition + offset;
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
}
