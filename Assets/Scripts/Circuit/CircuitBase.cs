using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CircuitBase : MonoBehaviour
{
    public abstract void Init();

    // Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    
    public static void BindEvent(GameObject go, Action<Vector3> action, Define.MouseEvent type = Define.MouseEvent.Click)
    {
        CircuitDragHandler handler = Util.GetOrAddComponent<CircuitDragHandler>(go);
        
        switch(type)
        {
            case Define.MouseEvent.BodyDrag:
                handler.OnDragHandler -= action;
                handler.OnDragHandler += action;
                break;
            
            case Define.MouseEvent.ConnectDrag:
                handler.OnDragHandler -= action;
                handler.OnDragHandler += action;
                break;
            
            case Define.MouseEvent.ConnectBeginDrag:
                handler.OnBeginDragHandler -= action;
                handler.OnBeginDragHandler += action;
                break;

            case Define.MouseEvent.ConnectEndDrag:
                handler.OnEndDragHandler -= action;
                handler.OnEndDragHandler += action;
                break;
        }
    }
}
