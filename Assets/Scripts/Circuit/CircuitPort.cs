using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CircuitPort : CircuitBase
{
    protected LineRenderer _line;
    protected Vector3 _startPosition;
    protected Vector3 _endPosition;
    protected GameObject _parentObject;
    protected bool _isDragging = false;

    protected AudioSource audioSource;

    [SerializeField]
    private AudioClip dragAudioClip;
    [SerializeField]
    private AudioClip dropAudioClip;
    [SerializeField]
    private AudioClip connectAudioClip;

    void Start()
    {
        Init();
    }
    public override void Init()
    {
        audioSource = GetComponent<AudioSource>();
        _parentObject = transform.parent.gameObject;
        BindEvent(_parentObject, OnBodyDrag, Define.MouseEvent.BodyDrag);
        BindEvent(gameObject, OnBeginDrag, Define.MouseEvent.ConnectBeginDrag);
        BindEvent(gameObject, OnDrag, Define.MouseEvent.ConnectDrag);
        BindEvent(gameObject, OnEndDrag, Define.MouseEvent.ConnectEndDrag);

        dragAudioClip = GameManager.Resource.Load<AudioClip>("Sounds/Effect/DragStart");
        dropAudioClip = GameManager.Resource.Load<AudioClip>("Sounds/Effect/DropFailed");
        connectAudioClip = GameManager.Resource.Load<AudioClip>("Sounds/Effect/Connected");
    }

    public abstract void OnBodyDrag(PointerEventData data);

    protected virtual void OnBeginDrag(PointerEventData data)
    {
        
        // Debug.Log("Port Begin Drag");
        _isDragging = true;

        GameObject _lineObject = new GameObject("Line");
        Transform _linesParent = _parentObject.transform.Find("Lines");
        if (_linesParent == null)
        {
            _linesParent = new GameObject("Lines").transform;
            _linesParent.transform.SetParent(transform.parent);
            _linesParent.transform.localPosition = Vector3.zero;
        }
        else
        {
            _lineObject.transform.SetParent(_linesParent);
        }

        _startPosition = transform.position;
        
        _line = _lineObject.transform.AddComponent<LineRenderer>();
        _line.positionCount = 2;
        _line.SetPosition(0, _startPosition);
        _line.SetPosition(1, _startPosition);
        _line.startColor = Color.white;
        _line.endColor = Color.white;
        _line.startWidth = 0.04f;
        _line.endWidth = 0.04f;
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.useWorldSpace = true;
    }

    protected virtual void OnDrag(PointerEventData data)
    {
        // Debug.Log("Port Dragging");

        if (_isDragging == false || _line == null) return;
        
        Vector3 currPos = data.position;
        currPos.z = 0f;
        _line.SetPosition(1, Camera.main.ScreenToWorldPoint(currPos));
    }

    protected abstract void OnEndDrag(PointerEventData data);
    public abstract void SetConnect(CircuitPort port, GameObject lineObject = null);
    public virtual void Disconnect()
    {
        if (_line != null)
        {
            Destroy(_line.gameObject);
            _line = null;
        }
    }

    protected virtual void LineColliderUpdate(GameObject go, Vector3 startPos, Vector3 endPos)
    {
        EdgeCollider2D edgeCollider;
        if (!go.TryGetComponent<EdgeCollider2D>(out edgeCollider))
        {
            go.AddComponent<EdgeCollider2D>();
            edgeCollider = go.GetComponent<EdgeCollider2D>();
            
            if (!edgeCollider.isTrigger)
            {
                edgeCollider.isTrigger = true;
            }
            
            int _lineLayer = LayerMask.NameToLayer("Line");
            edgeCollider.edgeRadius = 0.1f;
            edgeCollider.gameObject.layer = _lineLayer;
        };

        Vector2 localStart = go.transform.InverseTransformPoint(startPos);
        Vector2 localEnd = go.transform.InverseTransformPoint(endPos);

        List<Vector2> points = new List<Vector2>
        {
            localStart,
            localEnd
        };
        

        edgeCollider.SetPoints(points);
    }

    protected virtual void PlayDragSound()
    {
        audioSource.clip = dragAudioClip;
        audioSource.Play();
    }

    protected virtual void PlayDropSound()
    {
        audioSource.clip = dropAudioClip;
        audioSource.Play();
    }

    protected virtual void PlayConnectSound()
    {
        audioSource.clip = connectAudioClip;
        audioSource.Play();
    }
}
