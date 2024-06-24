using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public float TimeBeforeAffected; // The time after the object spawns until it will be affected by the timestop (for projectiles etc)
    private TimeManager timemanager;
    private Rigidbody rb;
    private Vector3 recordedVelocity;
    private float recordedMagnitude;

    private float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    private bool IsStopped;

    // Animation Handling
    private Animator animator;
    private bool wasAnimating;
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform player;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        animator = GetComponent<Animator>();
        TimeBeforeAffectedTimer = TimeBeforeAffected;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player").transform;


    }

    private void Update()
    {
        TimeBeforeAffectedTimer -= Time.deltaTime; // minus 1 per second
        if (TimeBeforeAffectedTimer <= 0f)
        {
            CanBeAffected = true; // Will be affected by timestop
        }

        if (CanBeAffected && timemanager.TimeIsStopped && !IsStopped)
        {
            if (rb.velocity.magnitude >= 0f) // If object is moving
            {
                recordedVelocity = rb.velocity.normalized; // Records direction of movement
                recordedMagnitude = rb.velocity.magnitude; // Records magnitude of movement

                rb.velocity = Vector3.zero; // Makes the rigidbody stop moving
                rb.isKinematic = true; // Not affected by forces
                if (agent != null && agent.isOnNavMesh)
                {
                    agent.isStopped = true;
                }
                IsStopped = true; // Prevents this from looping
                // agent.ResetPath();

                // Animation Handling
                if (animator != null)
                {
                    wasAnimating = animator.enabled;
                    animator.enabled = false; // Disable the animator
                }
            }
        }
    }

    public void ContinueTime()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = recordedVelocity * recordedMagnitude; // Adds back the recorded velocity when time continues
        }
        IsStopped = false;

        // Animation Handling
        if (animator != null)
        {
            animator.enabled = wasAnimating; // Restore the animator state
        }
        
        // Resume the NavMeshAgent
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
    }
}
