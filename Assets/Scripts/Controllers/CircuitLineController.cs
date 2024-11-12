using UnityEngine;

public class CircuitLineController : MonoBehaviour, ISelectable
{
    private bool _IsSelected = false;
    private LineRenderer _line = null;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    private void OnInteraction(Define.MouseEvent mouseEvent, Vector2 mousePos)
    {
        switch(mouseEvent)
        {
            case Define.MouseEvent.None:
                OnDeselect();
                break;

            case Define.MouseEvent.Click:
                OnSelect();
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
    }

    void OnDisable()
    {
        GameManager.Input.Unsubscribe(gameObject, OnInteraction);
    }

    public void OnSelect()
    {
        _IsSelected = true;

        Color pink = new Color(255 / 255f, 0 / 255f, 177 / 255f, 1f);
        _line.startColor = pink;
        _line.endColor = pink;
    }

    public void OnDeselect()
    {
        _IsSelected = false;

        _line.startColor = Color.white;
        _line.endColor = Color.white;
    }
}
