using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public float damage = 50f; // Adjust this value to set the amount of damage the explosion deals
    public float explosionRadius = 5f; // Adjust this value to set the explosion range
    public float explosionForce = 500f; // Adjust this value to set the force applied to objects in the explosion
    public LayerMask targetLayer; // Specify the layer that contains objects you want to apply damage to

    private void OnTriggerEnter(Collider other)
    {
        // When the SphereCollider triggers an overlap with an object
        // Check if the object is in the target layer and apply damage
        if (targetLayer == (targetLayer | (1 << other.gameObject.layer)))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(explosionForce * (other.transform.position - transform.position).normalized, ForceMode.Impulse);
            }

            Target target = other.GetComponent<Target>();
            EnemyAiTutorial enemy = other.GetComponent<EnemyAiTutorial>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
            else if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
