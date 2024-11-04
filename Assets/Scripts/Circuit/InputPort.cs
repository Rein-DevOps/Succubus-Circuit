using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputPort : CircuitPort
{
    private Signal _signal = null;
    private OutputPort _connectedOutput = null;
    public OutputPort ConnectedOutput => _connectedOutput;

    public override void OnBodyDrag(PointerEventData data)
    {
        Debug.Log("InputPort OnBodyDrag Called.");
        Debug.Log($"{_connectedOutput}");
        if (_connectedOutput == null) return;

        _connectedOutput.OnBodyDrag(data);
    }

    protected override void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("InputPort Begin Drag");

        Disconnect(_connectedOutput);
        
        base.OnBeginDrag(data);
    }

    protected override void OnEndDrag(PointerEventData data)
    {
        Debug.Log("InputPort End Drag");

        if (_isDragging == false || _line == null) return;

        LayerMask layerMask = LayerMask.GetMask("Outputport");
        Ray ray = Camera.main.ScreenPointToRay(data.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20.0f, layerMask);

        Debug.Log("InputPort Drag End");

        // OutputPort가 아닐 경우 연결 불가능
        if (hit.collider != null)
        {
            Debug.Log("InputPort Connected");
            OutputPort targetPort = hit.collider.GetComponent<OutputPort>();

            // 동일한 parent Object일 경우 연결 불가능
            if (transform.parent.gameObject == targetPort.transform.parent.gameObject)
            {
                // 추후 화면에 메시지 띄울 것
                Debug.Log("Same parent object");
                Destroy(_line.gameObject);
                _line = null;
                return;
            }

            _connectedOutput = targetPort;
            _line.SetPosition(1, targetPort.transform.position);
            SetConnect(targetPort, _line.gameObject);
        }
        else
        {
            Disconnect(_connectedOutput);
        }
        _isDragging = false;
    }

    public override void SetConnect(CircuitPort port, GameObject lineObject = null)
    {
        Debug.Log("Connect with OutputPort");
        if (port is OutputPort outputPort)
        {
            // OutputPort에서 InputPort에 연결 시도
            if (lineObject == null)
            {
                Disconnect();
                _connectedOutput = outputPort;
                // 
            }
            // InputPort에서 OutputPort에 연결 시도
            else
            {
                outputPort.SetConnect(this, lineObject);
            } 
        }
    }

    public void Disconnect(OutputPort outputPort)
    {
        // OutputPort와 연결되어있지 않을 경우
        if (outputPort == null)
        {
            Disconnect();
        }

        // 이미 OutputPort와 연결되어 있을 경우
        else
        {
            _connectedOutput = null;
            outputPort.Disconnect(this);
        }
    }
}
