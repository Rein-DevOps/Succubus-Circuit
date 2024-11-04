using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircuitBody : CircuitBase
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        GameObject go = transform.parent.gameObject;
        BindEvent(go, OnBodyDrag, Define.MouseEvent.BodyDrag);
    }

    private void OnBodyDrag(PointerEventData data)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(data.position);
        worldPosition.z = transform.position.z;
        transform.parent.position = worldPosition;
    }
}
