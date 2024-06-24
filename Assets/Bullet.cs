using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damageAmount = 10f; // Amount of damage to inflict
    public float pushPower = 10f;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is the player with a Health script
        HealthPlayer playerHealth = other.GetComponent<HealthPlayer>();
        if (playerHealth != null)
        {
            // Inflict damage on the player
            playerHealth.TakeDamage(damageAmount, pushPower, transform.position);

            // Destroy the bullet object
            Destroy(gameObject);
        }
    }
}