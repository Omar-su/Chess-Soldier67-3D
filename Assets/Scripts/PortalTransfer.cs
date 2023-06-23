using UnityEngine;

public class PortalTransfer : MonoBehaviour
{
    public Transform destinationPortal;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the portal is the player or the object you want to transfer
        if (other.CompareTag("Player") || other.CompareTag("TransferObject"))
        {
            // Calculate the relative position and rotation of the object in relation to the destination portal
            Vector3 relativePosition = destinationPortal.InverseTransformPoint(other.transform.position);
            Quaternion relativeRotation = Quaternion.Inverse(destinationPortal.rotation) * other.transform.rotation;

            // Teleport the object to the destination portal
            other.transform.position = destinationPortal.position + relativePosition;
            other.transform.rotation = destinationPortal.rotation * relativeRotation;
        }
    }
}
