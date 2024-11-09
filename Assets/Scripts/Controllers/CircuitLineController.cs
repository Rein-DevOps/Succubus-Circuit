using UnityEngine;


public class CircuitLineController : MonoBehaviour, IDraggable, IConnectable
{
    private bool IsSelected = false;

    void OnEnable()
    {
        if (GameManager.currScene == Define.Scene.Stage)
        {
            GameManager.Input.MouseAction -= OnMouseClicked;
            GameManager.Input.MouseAction += OnMouseClicked;
        }
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Clicked on UI element. Selection ignored.");
            return;
        }
        */
        
        // Debug.Log("On Mouse Click Called");
        if (evt == Define.MouseEvent.Select)
        {
            // Debug.Log("On Mouse Select Called");
            Deselect(selectedObject);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 20.0f, LayerMask.GetMask("Body") | LayerMask.GetMask("Line"));

            if (hit.collider != null)
            {
                int hitLayer = hit.collider.gameObject.layer;
                if (hitLayer == LayerMask.NameToLayer("Body"))
                {
                    // Debug.Log("On Body Select Called");
                    selectedType = SelectType.Body;
                    GameObject clickedBody = hit.transform.gameObject;
                    selectedObject = clickedBody;
                    
                    Transform selectedCircle = selectedObject.transform.parent.Find("SelectedCircle");
                    if (selectedCircle != null)
                    {
                        selectedCircle.gameObject.SetActive(true);
                    }
                    // Debug.Log("Gate Select Called!");
                    GameManager.Circuit.Select(selectedObject);
                }

                if (hitLayer == LayerMask.NameToLayer("Line"))
                {
                    // Debug.Log("On Line Select Called");
                    selectedType = SelectType.Line;
                    GameObject ClickedLine = hit.transform.gameObject;
                    selectedObject = ClickedLine;

                    LineRenderer line = selectedObject.GetComponent<LineRenderer>();
                    Color pink = new Color(255 / 255f, 0 / 255f, 177 / 255f, 1f);
                    line.startColor = pink;
                    line.endColor = pink;

                    // Debug.Log("Line Select Called!");
                    GameManager.Circuit.Select(selectedObject);
                }


            }


            /*
            else if (Physics2D.Raycast(mousePos, Vector2.zero, 20.0f, LayerMask.GetMask("Line")))
            {
                selectedType = SelectType.Line;
                GameObject clickedBody = hit.transform.gameObject;
            }
            */
        }

        if (evt == Define.MouseEvent.Click)
        {

        }
    }

    void Deselect(GameObject go = null)
    {
        if (go == null) return;

        Debug.Log("Deselect Called !");
        switch(selectedType)
        {
            case SelectType.None:
                break;

            case SelectType.Body:
                Transform selectedCircle = selectedObject.transform.parent.Find("SelectedCircle");
                selectedCircle.gameObject.SetActive(false);
                break;
            case SelectType.Line:
                LineRenderer line = selectedObject.GetComponent<LineRenderer>();
                line.startColor = Color.white;
                line.endColor = Color.white;
                break;
        }
        selectedType = SelectType.None;
        selectedObject = null;
    }



    private void HandleConnectBegin(Vector3 mousePos)
    {

    }

    private void HandleConnect(Vector3 mousePos)
    {

    }

    private void HandleConnectEnd(Vector3 mousePos)
    {

    }
}
