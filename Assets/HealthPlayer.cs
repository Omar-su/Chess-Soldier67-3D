using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{
    public float maxHealth = 200f; // Maximum health value
    private float currentHealth; // Current health value
    private Rigidbody playerRigidbody; // Reference to the player's rigidbody

    [Header("Damage Overlay")]
    public Image overlay; // our DamageOverlay Gameobject
    public float duration; // how long the image stays fully opaque
    public float fadeSpeed; // how quickly the image will fade
    private float durationTimer; // timer to check against the duration
    private void Start()
    {
        currentHealth = maxHealth; // Initialize the current health to the maximum health value
        playerRigidbody = GetComponent<Rigidbody>(); // Get the player's rigidbody component
        overlay.color = new Color (overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }

    private void Update() {
        Debug.Log($"health player: {currentHealth}");
        if (overlay.color.a > 0)
        {
            if(currentHealth < 30){
                return;
            }
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                // fade the image
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color (overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void TakeDamage(float damageAmount, float power, Vector3 damagingObjectPosition)
    {
        currentHealth -= damageAmount; // Decrease the current health by the damage amount

        // Check if the player's health has reached zero or below
        if (currentHealth <= 0f)
        {
            Die(); // Call the Die() method when the player's health is depleted
        }
        else
        {
            // Calculate the direction from the damaging object to the player
            Vector3 pushDirection = transform.position - damagingObjectPosition;
            pushDirection.y = 0f; // Optional: Set the y-component to 0 to prevent vertical displacement
            durationTimer = 0;
            overlay.color = new Color (overlay.color.r, overlay.color.g, overlay.color.b, 0.4f);
            // Apply a force to push the player back
            playerRigidbody.AddForce(pushDirection.normalized * power, ForceMode.Impulse);
        }
    }

    private void Die()
    {
        // Perform actions when the player dies, such as showing a game over screen or respawning
        Debug.Log("Player has died!");

        // Disable or destroy the player object, depending on your game's requirements
        gameObject.SetActive(false);
        // Alternatively, you can use Destroy(gameObject);
    }
}
