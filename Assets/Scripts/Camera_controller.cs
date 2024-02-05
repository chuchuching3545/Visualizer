using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_controller : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 lastMousePosition;
    public float dragSpeed = 0.07f;
    public float zoomSpeed = 1f; // 設定放大縮小速度

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            transform.position -= new Vector3(delta.x * dragSpeed, delta.y * dragSpeed, 0);
            lastMousePosition = Input.mousePosition;
        }

        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelInput != 0)
        {
            Camera.main.orthographicSize -= scrollWheelInput * zoomSpeed;
        }
    
    }
}
