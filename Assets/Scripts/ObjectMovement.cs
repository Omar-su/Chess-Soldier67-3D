using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public Transform targetObject;
    public float movementSpeed = 5f;
    private Vector3 originalPosition;
    private bool movingTowardsTarget = true;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (movingTowardsTarget)
        {
            MoveTowardsTarget();
        }
        else
        {
            MoveBackToOriginalPosition();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetDirection = targetObject.position - transform.position;
        Vector3 movement = targetDirection.normalized * movementSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        if (Vector3.Distance(transform.position, targetObject.position) <= movement.magnitude)
        {
            movingTowardsTarget = false;
        }

        // Rotate character to face the target object
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    private void MoveBackToOriginalPosition()
    {
        Vector3 originalDirection = originalPosition - transform.position;
        Vector3 movement = originalDirection.normalized * movementSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        if (Vector3.Distance(transform.position, originalPosition) <= movement.magnitude)
        {
            movingTowardsTarget = true;
        }

        // Rotate character to face its original position
        Quaternion originalRotation = Quaternion.LookRotation(originalDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, 10f * Time.deltaTime);
    }
}
