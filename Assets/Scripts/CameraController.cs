using UnityEngine;

public class CameraController : MonoBehaviour
{
<<<<<<< HEAD
    // === Zoom Settings ===
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 10f; 

    
    [Header("Panning Settings")]
    public float panSpeed = 20f;
    private Vector3 dragOrigin;
    private Camera mainCamera;
    private float originalCameraZ; 

    
    [Header("Optional Bounds")]
    
    public bool useBounds = false;
    public Vector2 minCameraPos = new Vector2(-10f, -10f);
    public Vector2 maxCameraPos = new Vector2(10f, 10f);

    void Start()
    {
        
        mainCamera = GetComponent<Camera>();

        if (mainCamera == null)
        {
            Debug.LogError("CameraController must be attached to a GameObject with a Camera component!");
            enabled = false;
            return;
        }

        
        mainCamera.orthographic = true;

        
        originalCameraZ = mainCamera.transform.position.z;
    }

    void Update()
    {
        HandleZoom();
        HandlePanning();
    }

    private void HandleZoom()
    {
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        
        if (scroll != 0f)
        {
            
            float newSize = mainCamera.orthographicSize - scroll * zoomSpeed;

            
            mainCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }

    private void HandlePanning()
    {
        
        if (Input.GetMouseButtonDown(1)) 
        {
            
            dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        
        else if (Input.GetMouseButton(1)) 
        {
            
            Vector3 currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            
            Vector3 delta = dragOrigin - currentPosition;

            
            Vector3 targetPosition = mainCamera.transform.position + delta;

            
            targetPosition.z = originalCameraZ;

            
            if (useBounds)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, minCameraPos.x, maxCameraPos.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minCameraPos.y, maxCameraPos.y);
            }

            
            mainCamera.transform.position = targetPosition;
        }
=======
    public float speed = 5f;
    public float zoomSpeed = 2f;
    Camera cam;

    void Start() => cam = GetComponent<Camera>();

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, moveY, 0));

        cam.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 3f, 10f);
>>>>>>> 8bc831299161ad3aa5f47b3508df185dd8833585
    }
}


