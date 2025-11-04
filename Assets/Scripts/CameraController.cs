using UnityEngine;

public class CameraController : MonoBehaviour
{
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
    }
}
