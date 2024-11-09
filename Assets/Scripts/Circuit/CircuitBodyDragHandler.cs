using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class CircuitBodyDragHandler : IDraggable
{   
    public Action<Vector3> OnDragHandler = null;
    public Action<Vector3> OnBeginDragHandler = null;
    public Action<Vector3> OnEndDragHandler = null;

    public void OnStartDrag(Vector3 eventData)
    {
        // Debug.Log("OnBeginDrag Called");
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke(eventData);
    }

    public void OnDrag(Vector3 eventData)
    {
        // Debug.Log("OnDrag Called");
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }


    public void OnEndDrag(Vector3 eventData)
    {
        // Debug.Log("OnEndDrag Called");
        if (OnEndDragHandler != null)
            OnEndDragHandler.Invoke(eventData);
    }
}
