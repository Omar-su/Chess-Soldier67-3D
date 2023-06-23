using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 rotationSpeed; // Speed of rotation around each axis

    void Update()
    {
        // Rotate the object based on the rotation speed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
