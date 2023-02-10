using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update

    public float health = 50f;
    public GameObject dissolveEffect;

    public void TakeDamage (float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            GameObject impactGO = Instantiate(dissolveEffect, transform);
            Destroy(impactGO, 1f);
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject, 2f);
    }
} 
