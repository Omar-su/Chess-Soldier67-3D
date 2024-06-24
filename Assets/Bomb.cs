using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject projectile;
    public ParticleSystem explosionEffect; // Assign the Particle System prefab to this in the Inspector

    private bool canShoot = true;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;

    private List<GameObject> instantiatedProjectiles = new List<GameObject>();
    private int maxBombs = 10;
    private int remainingBombs = 10;
    public AudioSource audioSource;
    public AudioClip bombExplosion;
    public AudioClip bombRelease;
    public Camera fpscamera;
    public float volume = .5f;
    public Transform firepoint;
    public float attackPower = 300f;
    public float maxDistance = 2000000000f;
    public float damage = 50f; // Adjust this value to set the amount of damage the explosion deals
    public float explosionRadius = 5f; // Adjust this value to set the explosion range
    public float explosionForce = 500f; // Adjust this value to set the force applied to objects in the explosion
    public LayerMask targetLayer; // Specify the layer that contains objects you want to apply damage to
    private TimeManager timemanager;
    private Transform targetEnemy;
    private Rigidbody bombRigidbody;
    private void Start() {
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && canShoot)
        {
            if (remainingBombs > 0)
            {
                // Set the target enemy before shooting
                // SetTargetEnemy();
                // if (targetEnemy != null)
                // {
                //     Shoot();
                //     remainingBombs--;
                // }

                Shoot();
                remainingBombs--;
                
            }
        }

        if (Input.GetMouseButtonDown(1) && !timemanager.TimeIsStopped)
        {
            DetonateProjectile();
        }

        // if(targetEnemy!=null && bombRigidbody != null)
        // {
        //     float speed = bombRigidbody.velocity.magnitude;
        //     // Direct towards the player
        //     bombRigidbody.transform.LookAt(targetEnemy.position);

        //     // Move towards the player
        //     bombRigidbody.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // }
        
    }


    // void SetTargetEnemy()
    // {
    //     RaycastHit hit;

    //     if (Physics.Raycast(fpscamera.transform.position, fpscamera.transform.forward, out hit))
    //     {
            
    //         //print something
    //         Debug.Log(hit.collider.gameObject.name);
    //         // Check if the hit object is an enemy
    //         if (hit.collider.gameObject.layer == LayerMask.NameToLayer("enemy"))
    //         {
    //             targetEnemy = hit.collider.transform;
    //         }
    //     }
    // }

    void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            audioSource.PlayOneShot(bombRelease, volume);
            GameObject instantiatedProjectile = Instantiate(projectile, firepoint.position, Quaternion.identity);
            instantiatedProjectiles.Add(instantiatedProjectile);
            Rigidbody rb = instantiatedProjectile.GetComponent<Rigidbody>();
            bombRigidbody = rb;
            Vector3 forwardDirection = firepoint.forward;
            
            // Start a coroutine to wait until TimeIsStopped becomes false or the key is pressed
            StartCoroutine(WaitForTimeToResume(rb, forwardDirection));
             // Update the next fire time
            nextFireTime = Time.time + cooldownTime;

            // Start the cooldown coroutine
            StartCoroutine(CooldownCoroutine());
        }
    }


    IEnumerator WaitForTimeToResume(Rigidbody rb, Vector3 forceDirection)
    {
        while (timemanager.TimeIsStopped)
        {
            yield return null;
        }

        // Now TimeIsStopped is false (or the key is pressed), add the force
        rb.AddForce(forceDirection * attackPower, ForceMode.Impulse);

        
    }

    void DetonateProjectile()
    {
        if (instantiatedProjectiles.Count > 0)
        {
            GameObject projectileToDetonate = instantiatedProjectiles[0];

            // Play the explosion effect
            ParticleSystem explosionInstance = Instantiate(explosionEffect, projectileToDetonate.transform.position, Quaternion.identity);
            explosionInstance.Play();
            // audioSource.PlayOneShot(bombExplosion, volume);
            // Get the distance between the explosion and the player
            float distanceToPlayer = Vector3.Distance(projectileToDetonate.transform.position, transform.position);

            // Store the original pitch
            float originalPitch = audioSource.pitch;
            // Play the explosion sound with distance-based properties
            audioSource.pitch = Mathf.Lerp(0.5f, 1.5f, distanceToPlayer / maxDistance);
            volume = Mathf.Clamp01(1f - (distanceToPlayer / maxDistance));
            audioSource.PlayOneShot(bombExplosion, volume);
            audioSource.pitch = originalPitch;
            volume = 1f;
            // Apply damage to objects in the specified layer within the explosion radius
            Collider[] colliders = Physics.OverlapSphere(projectileToDetonate.transform.position, explosionRadius, targetLayer);

            foreach (Collider c in colliders)
            {
                Rigidbody rb = c.GetComponent<Rigidbody>();
                UnityEngine.AI.NavMeshAgent agent = c.GetComponent<UnityEngine.AI.NavMeshAgent>();

                // if (rb != null)
                // {
                //     if (agent != null && agent.isOnNavMesh)
                //     {
                //         agent.isStopped = true;
                //     }
                //     rb.AddForce(explosionForce * (c.transform.position - projectileToDetonate.transform.position).normalized, ForceMode.Impulse);
                // }
                if (agent == null)
                {
                    rb.AddForce(explosionForce * (c.transform.position - projectileToDetonate.transform.position).normalized, ForceMode.Impulse);
                }   

                Target target = c.GetComponent<Target>();
                EnemyAiTutorial enemy = c.GetComponent<EnemyAiTutorial>();
                HealthSystem healthSystem = c.GetComponent<HealthSystem>();

                if (target != null)
                {
                    target.TakeDamage(damage);
                }
                else if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }else if(healthSystem != null) {
                    healthSystem.TakeDamage(damage);
                }
                
            }

            // Destroy the effect once it finishes playing
            Destroy(explosionInstance.gameObject, explosionInstance.main.duration);

            Destroy(projectileToDetonate);
            instantiatedProjectiles.RemoveAt(0);
            remainingBombs++;
        }
    }

    IEnumerator CooldownCoroutine()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldownTime);
        canShoot = true;
    }

        // This method is called in the Unity Editor and draws the explosion sphere
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
