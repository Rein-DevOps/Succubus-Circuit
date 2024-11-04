using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private GateSpawner gateSpawner;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    gateSpawner.SpawnGate(hit.transform);
                }
            }
        }
    }
}
