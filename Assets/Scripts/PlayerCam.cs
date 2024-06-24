using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public float rotationSmoothTime = 0.1f;

    public Transform orientation;

    float xRotation;
    float yRotation;
    Vector2 currentRotation;

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Smooth the rotation
        currentRotation = Vector2.SmoothDamp(currentRotation, new Vector2(xRotation, yRotation), ref currentRotation, rotationSmoothTime);

        // Rotate camera and orientation
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
        orientation.rotation = Quaternion.Euler(0, currentRotation.y, 0);
    }
}
