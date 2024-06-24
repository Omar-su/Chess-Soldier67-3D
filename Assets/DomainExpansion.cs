using System.Collections;
using UnityEngine;

public class DomainExpansion : MonoBehaviour
{
    public GameObject spherePrefab;  // Prefab of the sphere
    public GameObject domainPrefab;  // Prefab of the domain

    public int sphereCount = 3;     // Number of spheres to spawn
    public float sphereRadius = 2f;  // Radius of the smaller spheres

    public float rotationSpeed = 30f;  // Rotation speed of the spheres

    private bool spheresRotating = false;

    // Update is called once per frame
    void Update()
    {
        // If P is pressed then instantiate the spheres and start rotation
        if (Input.GetKeyDown(KeyCode.P) && !spheresRotating)
        {
            RotateSpheres();
            StartCoroutine(InstantiateDomain());
        }
    }

    void RotateSpheres()
    {
        spheresRotating = true;



        // Create 10 spheres around the central sphere
        for (int i = 0; i < sphereCount; i++)
        {
            float angle = i * (360f / sphereCount);
            Vector3 offset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle)) * sphereRadius;
            Instantiate(spherePrefab, transform.position + offset, Quaternion.identity);
        }


        // while (true)
        // {
        //     // Rotate all spheres around the object with this script
        //     centerSphere.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        //     foreach (Transform child in transform)
        //     {
        //         if (child != centerSphere.transform && child != bigSphere.transform)
        //         {
        //             child.transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        //         }
        //     }

        //     yield return null;
        // }
    }

    IEnumerator InstantiateDomain()
    {
        yield return new WaitForSeconds(1f);  // Wait for 1 second

        // Create the domain prefab
        Instantiate(domainPrefab, transform.position, Quaternion.identity);
    }
}
