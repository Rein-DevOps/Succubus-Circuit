using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutputPort : CircuitPort
{
    private Signal _signal;
    private List<InputPort> _connectedInputs = new List<InputPort>();
    private List<LineRenderer> _lines = new List<LineRenderer>();

    public List<InputPort> ConnectedInputs => _connectedInputs;
    public List<LineRenderer> Lines => _lines;

    public override void OnBodyDrag(PointerEventData data)
    {
        // Debug.Log("BodyDrag of Output Called");
        for (int i = 0; i < _connectedInputs.Count; i++)
        {
            if (_lines[i].GetPosition(0) != new Vector3(data.position.x, data.position.y, 0))
            {
                _lines[i].SetPosition(0, _connectedInputs[i].transform.position);
                _lines[i].SetPosition(1, transform.position);

                LineColliderUpdate(_lines[i].gameObject, _lines[i].GetPosition(0), _lines[i].GetPosition(1));
            }
        }
    }
    protected override void OnEndDrag(PointerEventData data)
    {
        // Debug.Log("OutputPort End Drag");

        if (_isDragging == false || _line == null) return;

        LayerMask layerMask = LayerMask.GetMask("Inputport");
        Ray ray = Camera.main.ScreenPointToRay(data.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20.0f, layerMask);

        // Debug.Log("OutputPort Drag End");

        // InputPort가 아닐 경우 연결 불가능
        if (hit.collider != null)
        {
            // Debug.Log("OutputPort Connected");
            InputPort targetPort = hit.collider.GetComponent<InputPort>();
            
            // 동일한 parent Object일 경우 연결 불가능
            if(transform.parent.gameObject == targetPort.transform.parent.gameObject)
            {
                // 추후 화면에 메시지 띄울 것
                // Debug.Log("Same parent object");
                Disconnect();
                PlayDropSound();
                return;
            }

            // 동일한 InputPort에 연결할 경우 연결 불가능
            if (_connectedInputs.Contains(targetPort))
            {
                // Debug.Log("Trying connect to same line");
                Disconnect();
                PlayDropSound();
                return;
            }

            SetConnect(targetPort);
            PlayConnectSound();
        }
        else
        {
            Disconnect();
        }

        _isDragging = false;
    }

    public override void SetConnect(CircuitPort port, GameObject lineObject = null)
    {
        if (port is InputPort inputPort)
        {
            _connectedInputs.Add(inputPort);

            // OutputPort에서 InputPort에 연결 시도
            if (lineObject == null)
            {
                _line.SetPosition(0, inputPort.transform.position);
                _line.SetPosition(1, transform.position);
                _lines.Add(_line);
                inputPort.SetConnect(this);
                // LineColliderUpdate(_line.gameObject, transform.position, inputPort.transform.position);
            }
            // InputPort에서 OutputPort에 연결 시도
            else
            {
                lineObject.transform.SetParent(_parentObject.transform.Find("Lines"));
                _lines.Add(lineObject.GetComponent<LineRenderer>());
                // LineColliderUpdate(lineObject, transform.position, inputPort.transform.position);
            }

            // lineObject에 Collider 추가
            LineColliderUpdate(_lines[_lines.Count -1].gameObject, transform.position, inputPort.transform.position);
        }
    }

    public void DisconnectAll()
    {
        for (int i = _connectedInputs.Count - 1; i >= 0; i--)
        {
            Disconnect(_connectedInputs[i]);
        }
    }

    public void Disconnect(InputPort inputPort)
    {
        int index = _connectedInputs.IndexOf(inputPort);
        if (index != -1)
        {
            _connectedInputs.RemoveAt(index);
            if (index < _lines.Count)
            {
                Destroy(_lines[index].gameObject);
                _lines.RemoveAt(index);
            }

            // Debug.Log("OutputPort Disconnected from inputPort");
        }
    }

    // 특정 LineRenderer와 연결된 InputPort를 찾아 Disconnect
    public void DisconnectLine(LineRenderer lineRenderer)
    {
        int index = _lines.IndexOf(lineRenderer);
        if (index != -1 && index < _connectedInputs.Count)
        {
            InputPort inputPort = _connectedInputs[index];
            Disconnect(inputPort);
        }
        else
        {
            Debug.LogWarning("LineRenderer not found in _lines list.");
        }
    }

    public bool HasLine(LineRenderer lineRenderer)
    {
        return _lines.Contains(lineRenderer);
    }
    
    public void UpdateLinePosition()
    {
        // Debug.Log("Outport Update Position Called");
        if (_line != null && _connectedInputs.Count > 0 && _lines.Count == _connectedInputs.Count)
        {
            // Debug.Log("Outport Line is not null");

            for (int i = 0; i < _lines.Count; i++){
                InputPort connectedInput = _connectedInputs[i];
                LineRenderer line = _lines[i];

                if (connectedInput != null && line != null)
                {
                    Vector3 startPos = connectedInput.transform.position;
                    Vector3 endPos = transform.position; // 끝점 (Output)

                    line.SetPosition(0, startPos);
                    line.SetPosition(1, endPos);
                }
            }
        }
    }
}
