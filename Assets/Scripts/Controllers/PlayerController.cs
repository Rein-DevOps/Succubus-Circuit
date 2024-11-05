using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public GameObject selectedObject = null;
    private enum SelectedType
    {
        None,
        Body,
        Line
    }

    SelectedType selectedType = SelectedType.None;

    void Start()
    {
        GameManager.Input.MouseAction -= OnMouseClicked;
        GameManager.Input.MouseAction += OnMouseClicked;
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

            //int bodyLayerMask = LayerMask.GetMask("Body");
            //Debug.Log("Body LayerMask: " + bodyLayerMask);  // LayerMask 값 확인
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 20.0f, LayerMask.GetMask("Body") | LayerMask.GetMask("Line"));

            if (hit.collider != null)
            {
                int hitLayer = hit.collider.gameObject.layer;
                if (hitLayer == LayerMask.NameToLayer("Body"))
                {
                    // Debug.Log("On Body Select Called");
                    selectedType = SelectedType.Body;
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
                    selectedType = SelectedType.Line;
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
                selectedType = SelectedType.Line;
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
            case SelectedType.None:
                break;

            case SelectedType.Body:
                Transform selectedCircle = selectedObject.transform.parent.Find("SelectedCircle");
                selectedCircle.gameObject.SetActive(false);
                break;
            case SelectedType.Line:
                LineRenderer line = selectedObject.GetComponent<LineRenderer>();
                line.startColor = Color.white;
                line.endColor = Color.white;
                break;
        }
        selectedType = SelectedType.None;
        selectedObject = null;
    }
}
