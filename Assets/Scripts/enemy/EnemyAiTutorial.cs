
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public Animator animator;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check for sight range
        RaycastHit sightHit;
        bool playerInSightRange = Physics.Raycast(transform.position, player.position - transform.position, out sightHit, sightRange, whatIsPlayer);
 
        // Check for attack range
        RaycastHit attackHit;
        bool playerInAttackRange = Physics.Raycast(transform.position, player.position - transform.position, out attackHit, attackRange, whatIsPlayer);
   

        if (playerInAttackRange && playerInSightRange) StartCoroutine("AttackPlayer");

        else if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        else Patroling();
    }

    private void Patroling()
    {
        // Stop moving and trigger idle animation
        animator.SetBool("run", false);
        animator.SetBool("patrol", true);
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {        
        // Stop moving and trigger idle animation
        animator.SetBool("patrol", false);
        // Stop moving and trigger idle animation
        animator.SetBool("run", true);
        agent.SetDestination(player.position);
    }

    // Called by an animation event
    IEnumerator AttackPlayer()
    {
        // Stop moving and trigger idle animation
        animator.SetBool("run", false);
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            
            // Trigger shooting animation
            animator.SetTrigger("fire");
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 100f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        yield return new WaitForSeconds(.7f);
    }

    // }
    // private void AttackPlayer()
    // {   // Stop moving and trigger idle animation
    //     animator.SetBool("run", false);
    //     //Make sure enemy doesn't move
    //     agent.SetDestination(transform.position);

    //     transform.LookAt(player);

    //     if (!alreadyAttacked)
    //     {
            
    //         // Trigger shooting animation
    //         animator.SetTrigger("fire");
    //         ///Attack code here
    //         Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
    //         rb.AddForce(transform.forward * 100f, ForceMode.Impulse);
    //         rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    //         ///End of attack code

    //         alreadyAttacked = true;
    //         Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //     }
    // }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        // Trigger shooting animation
        animator.SetTrigger("dead");
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
