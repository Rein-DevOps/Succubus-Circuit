using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircuitHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnBeginDragHandler = null;
    public Action<PointerEventData> OnEndDragHandler = null;

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag Called");
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag Called");
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag Called");
        if (OnEndDragHandler != null)
            OnEndDragHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnClick called");
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }
}
