using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;  // Reference to the character's transform
    public float movementSpeed = 3f;
    public float shootingRange = 10f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float shootingInterval = 2f;
    public float deathDelay = 2f;
    public Animator animator;

    private bool isShooting = false;
    private float nextShootTime = 0f;
    private bool isDead = false;

    private void Update()
    {
        if (isDead)
            return;

        // Calculate the distance between enemy and character
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Enemy follows the character if within shooting range
        if (distanceToTarget <= shootingRange)
        {

            if(!isShooting){
                transform.LookAt(target);
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
            }

            // Trigger movement animation
            animator.SetBool("run", true);

            // Check if it's time to shoot
            if (Time.time >= nextShootTime && !isShooting && distanceToTarget < 20)
            {
                // Stop moving and trigger idle animation
                animator.SetBool("run", false);

                // Trigger shooting animation
                animator.SetTrigger("fire");
                isShooting = true;

                StartCoroutine("ShootProjectile");

                // Calculate the time for the next shot
                nextShootTime = Time.time + shootingInterval;
            }
        }
        else
        {
            // Stop moving and trigger idle animation
            animator.SetBool("run", false);
        }
    }


    // Called by an animation event
    IEnumerator ShootProjectile()
    {
        // Instantiate and shoot the projectile
        Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        yield return new WaitForSeconds(.7f);
        isShooting = false;
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("dead");

        // Delay before destroying the enemy object
        Invoke("DestroyEnemy", deathDelay);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
