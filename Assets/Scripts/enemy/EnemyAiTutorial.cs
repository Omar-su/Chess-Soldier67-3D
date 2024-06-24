using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public Transform firepoint;
    private Transform targetPoint;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    bool isAttacking = false;
    bool alive = true;
    bool canMove = true;
    public float aimingOffset = 0.05f; // Adjust this value to fine-tune the aiming

    public float attackPower = 300f;
    public float syncAttack = .7f;
    private float damageAmount = 0;
    public GameObject floatingText;
    private Coroutine attackCoroutine;
    private TimeManager timeManager;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        if(player) {
            targetPoint = player.Find("targetPoint");
        }
        timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        if (timeManager == null)
        {
            Debug.LogError("TimeManager not found");
        }
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
   

        if(alive){
            // Check for sight range
            RaycastHit sightHit;
            playerInSightRange = Physics.Raycast(transform.position, player.position - transform.position, out sightHit, sightRange, whatIsPlayer);

            // Check for attack range
            RaycastHit attackHit;
            playerInAttackRange = Physics.Raycast(transform.position, player.position - transform.position, out attackHit, attackRange, whatIsPlayer);

            if (playerInAttackRange && alive && !timeManager.TimeIsStopped) {
                // Start or resume attacking coroutine
                if (attackCoroutine == null)
                {
                    attackCoroutine = StartCoroutine(AttackPlayer());
                }
            }else{
                       // Stop attacking coroutine when not in attack range or time is stopped
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                }
            }
            if (!playerInAttackRange && !isAttacking && alive) ChasePlayer();
            // else if (alive) Patroling();
        } else {
            agent.ResetPath();
            agent.isStopped = true;
        }

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
        // if (!canMove)
        //     return;

        // Stop moving and trigger idle animation
        animator.SetBool("patrol", false);
        animator.SetBool("run", true);
        agent.SetDestination(player.position);
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        canMove = false; // Disable movement

        // Stop moving and trigger idle animation
        animator.SetBool("run", false);
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Trigger shooting animation
            animator.SetTrigger("fire");
            yield return new WaitForSeconds(syncAttack);
            ///Attack code here
            // Instantiate the projectile at the character's position
            GameObject instantiatedProjectile = Instantiate(projectile, firepoint.position, Quaternion.identity);
            Rigidbody rb = instantiatedProjectile.GetComponent<Rigidbody>();

            // Calculate the direction from the character to the target point
            Vector3 direction = (targetPoint.position - transform.position).normalized;

            // Apply a force in the adjusted direction towards the target point
            rb.AddForce(direction * attackPower, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        yield return new WaitForSeconds(.7f);
        isAttacking = false;
        canMove = true; // Enable movement again after attacking
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        if(!alive) return;
        health -= damage;
        if(floatingText && health > 0) ShowFloatingText();
        Debug.Log($"Health of enemy: {health}");
        if (health <= 0) DestroyEnemy();
    }

    private void ShowFloatingText()
    {
        var text = Instantiate(floatingText, transform.position, Quaternion.Euler(0f, 180f, 0f), transform);
        text.GetComponent<TextMesh>().text = health.ToString(); 
    }

    private void DestroyEnemy()
    {
        // Trigger shooting animation
        alive = false;
        // Stop the agent and reset its path
        agent.isStopped = true;
        agent.ResetPath();
        // Freeze the Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.velocity = Vector3.zero;
        }
        // Disable the collider
        Collider enemyCollider = GetComponent<Collider>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
        animator.SetTrigger("dead");
        Destroy(gameObject, 10f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
