using UnityEngine;

public class MoonOrbit : MonoBehaviour
{
    public Transform sun; // Reference to the Sun object
    public float orbitSpeed = 1f; // Speed of rotation around the sun

    private void Update()
    {
        // Rotate around the sun
        transform.RotateAround(sun.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
