using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update

    public float health = 50f;
    public GameObject dissolveEffect;

    public void TakeDamage (float amount)
    {
        health -= amount;
        // Debug.Log($"Health now is: {health}");
        if (health <= 0f)
        {
            GameObject impactGO = Instantiate(dissolveEffect, transform.position, Quaternion.identity);
            impactGO.transform.parent = transform;
            Destroy(impactGO, 2f);
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject, 1f);
    }
} 
