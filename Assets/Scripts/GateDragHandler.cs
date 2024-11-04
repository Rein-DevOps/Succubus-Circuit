using UnityEngine;
using UnityEngine.EventSystems;

public class GateDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject gatePrefab;
    private GameObject draggingGate;
    private Canvas canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
    
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
