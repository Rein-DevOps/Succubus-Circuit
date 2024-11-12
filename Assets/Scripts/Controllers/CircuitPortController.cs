using System.Collections;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CircuitPortController : MonoBehaviour, IMouseInteractable, IConnectable
{
    enum Porttype
    {
        None,
        InputPort,
        OutputPort
    }

    private GameObject parentObject = null;
    private Porttype currPort = Porttype.None;

    private Vector2 lineStartPosition = new Vector2();
    private Vector2 lineEndPosition = new Vector2();

    private Transform lineParent = null;
    private GameObject lineObject = null;
    private GameObject lineStartObject = null;
    private GameObject lineEndObject = null;
    private LineRenderer line = null;
    private EdgeCollider2D edgeCollider = null;

    private float edgeColliderOffset = 0.2f; // Port의 Collider와 겹치지 않게 하기 위하여


    void Awake()
    {
        parentObject = transform.parent.gameObject;

        if (lineParent == null)
        {
            lineParent = transform.parent.Find("Lines");

            if (lineParent == null)
            {
                Debug.LogWarning("Cannot Find 'Lines'");
            }
        }

        if (gameObject.layer == LayerMask.NameToLayer("InputPort"))
        {
            currPort = Porttype.InputPort;
        }
        else if(gameObject.layer == LayerMask.NameToLayer("OutputPort"))
        {
            currPort = Porttype.OutputPort;
        }
        else
        {
            Debug.LogWarning("CircutPort Type is not assigned. Please Check Layer and code.");
        }
    }

    private void OnInteraction(Define.MouseEvent mouseEvent, Vector2 mousePos)
    {
        switch (mouseEvent)
        {
            case Define.MouseEvent.Click:
                OnClick(mousePos);
                break;
            case Define.MouseEvent.Press:
                OnPress(mousePos);
                break;
            case Define.MouseEvent.Release:
                OnRelease(mousePos);
                Reset();
                break;
            default:
                break;
        }
    }

    void OnEnable()
    {
        if (GameManager.currScene == Define.Scene.Stage)
        {
            GameManager.Input.Subscribe(gameObject, OnInteraction);
        }

        switch (currPort)
        {
            case Porttype.InputPort:
                GameManager.Circuit.InputPortSet(parentObject, gameObject);
                break;

            case Porttype.OutputPort:
                GameManager.Circuit.OutputPortSet(parentObject, gameObject);
                break;
        }
    }

    void OnDisable()
    {
        GameManager.Input.Unsubscribe(gameObject, OnInteraction);

        switch (currPort)
        {
            case Porttype.InputPort:
                GameManager.Circuit.InputPortUnset(parentObject, gameObject);
                break;

            case Porttype.OutputPort:
                GameManager.Circuit.OutputPortUnset(parentObject, gameObject);
                break;
        }

        DisConnect();
    }

    public void OnClick(Vector2 mousePosition)
    {
        lineStartPosition = transform.position;
        lineEndPosition = transform.position;
        lineObject = LineGenerate(lineStartPosition, lineEndPosition);
    }

    public void OnPress(Vector2 mousePosition)
    {
        switch (currPort)
        {
            case Porttype.InputPort:
                lineStartPosition = mousePosition;
                break;
            case Porttype.OutputPort:
                lineEndPosition = mousePosition;
                break;
            default:
                break;
        }
        LineUpdate(lineStartPosition, lineEndPosition);
    }

    public void OnRelease(Vector2 mousePosition)
    {
        Porttype mousePositionPort = Porttype.None;

        switch (currPort)
        {
            case Porttype.InputPort:
                lineStartPosition = mousePosition;
                break;
            case Porttype.OutputPort:
                lineEndPosition = mousePosition;
                break;
            default:
                break;
        }

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 20.0f, LayerMask.GetMask("InputPort", "OutputPort"));
        if (GameManager.Input.IsPointerOverCircuitInputPort(hit)) mousePositionPort = Porttype.InputPort;
        if (GameManager.Input.IsPointerOverCircuitOutputPort(hit)) mousePositionPort = Porttype.OutputPort;

        if (LineSet(lineStartPosition, lineEndPosition, hit, mousePositionPort))
        {
            // Connect(hit.collider.transform.parent.gameObject.GetComponent<GateBase>());
            Connect(hit.collider.gameObject);
        }
        else
        {
            Destroy(lineObject);
        }
    }   

    public void Connect(GameObject targetPort)
    {
        switch (currPort)
        {
            // InputPort일 경우 현재 연결되어있는 line connect 제거
            case Porttype.InputPort:
                if (GameManager.Circuit.connectports.ContainsKey(gameObject))
                {
                    
                }
                break;

            // OutputPort일 경우 연결 시도하는 InputPort가 다른 port와 연결되어 있을 경우 제거
            case Porttype.OutputPort:
                if (GameManager.Circuit.lines.ContainsKey(targetPort))
                    targetPort.GetComponent<CircuitPortController>().DisConnect();
                break;
        }

        // CircuitManager에 Connect된 line을 제어하도록 호출
        GameManager.Circuit.LineConnect(lineStartObject, lineEndObject, lineObject);
        GameManager.Circuit.PortConnect(lineStartObject, lineEndObject);
        // GameManager.Circuit.GateConnect(parentObject.GetComponent<GateBase>(), targetGate);
        // GameManager.Circuit.GateConnect(targetGate, parentObject.GetComponent<GateBase>());
    }

    public void DisConnect()
    {
        // CircuitManager에 Connect된 line을 Disconnect하도록 호출
        switch (currPort)
        {
            case Porttype.InputPort:

                break;

            case Porttype.OutputPort:
                break;
        }
    }

    private void Reset()
    {
        lineObject = null;
        line = null;
        edgeCollider = null;
        lineStartObject = null;
        lineEndObject = null;
    }

    GameObject LineGenerate(Vector2 lineStartPosition, Vector2 lineEndPosition)
    {
        Vector2 lineDir = lineEndPosition - lineStartPosition;
        lineDir = lineDir.normalized;

        GameObject linePrefab = Resources.Load<GameObject>("Prefabs/Gates/Line");

        if (linePrefab == null)
        {
            Debug.LogError("Line Prefab Not Found!");
            return null;
        }

        GameObject go = Instantiate(linePrefab, lineParent);

        line = go.GetComponent<LineRenderer>();
        edgeCollider = go.GetComponent<EdgeCollider2D>();

        line.SetPositions(new Vector3[] {lineStartPosition, lineEndPosition});

        Vector2[] colliderPositions = new Vector2[2];
        
        colliderPositions[0] = lineStartPosition + lineDir * edgeColliderOffset;
        colliderPositions[1] = lineEndPosition - lineDir * edgeColliderOffset;
        edgeCollider.points = colliderPositions;

        return go;
    }

    void LineUpdate(Vector2 lineStartPosition, Vector2 lineEndPosition)
    {
        Vector2 lineDir = lineEndPosition - lineStartPosition;
        lineDir = lineDir.normalized;

        if (line != null && edgeCollider != null){
            line.SetPositions(new Vector3[] {lineStartPosition, lineEndPosition});

            Vector2[] colliderPositions = new Vector2[2];
    
            colliderPositions[0] = lineStartPosition + lineDir * edgeColliderOffset;
            colliderPositions[1] = lineEndPosition - lineDir * edgeColliderOffset;
            edgeCollider.points = colliderPositions;
        }
    }

    bool LineSet(Vector2 lineStartPosition, Vector2 lineEndPosition, RaycastHit2D hit, Porttype mousePositionPort)
    {
        if (currPort == mousePositionPort || mousePositionPort == Porttype.None)
        {
            return false;
        }
        
        if (hit.collider == null)
        {
            return false;
        }
        
        if (hit.collider.transform.parent == lineParent.parent)
        {
            return false;
        }
    
        switch (currPort)
        {
            case Porttype.InputPort:
                lineStartPosition = hit.collider.transform.position;
                lineStartObject = hit.collider.gameObject;
                lineEndObject = gameObject;



                break;

            case Porttype.OutputPort:
                lineEndPosition = hit.collider.transform.position;
                lineStartObject = gameObject;
                lineEndObject = hit.collider.gameObject;
                break;
            default:
                return false;
        }

        Vector2 lineDir = lineEndPosition - lineStartPosition;
        lineDir = lineDir.normalized;

        if (line != null && edgeCollider != null){
            line.SetPositions(new Vector3[] {lineStartPosition, lineEndPosition});
            
            Vector2[] colliderPositions = new Vector2[2];
            
            colliderPositions[0] = lineStartPosition + lineDir * edgeColliderOffset;
            colliderPositions[1] = lineEndPosition - lineDir * edgeColliderOffset;

            edgeCollider.points = colliderPositions;
        }

        return true;
    }
}
