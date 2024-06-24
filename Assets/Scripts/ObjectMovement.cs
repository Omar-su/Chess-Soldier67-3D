using System.Collections;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
   private Animator bossAnimator;
    private Transform player;
    public float detectionRange = 400f;
    public LayerMask playerLayer;
    private AudioSource audioSource;
    public UnityEngine.AI.NavMeshAgent agent;

    private bool isPlayerInRange = false;
    public float walkingSpeed = 2.0f; // Adjust the speed as needed

    private TimeManager timeManager;
    private Transform targetPoint;

    public UnityEngine.AI.NavMeshAgent navMeshAgent;

    public float shootingRange = 300f;
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
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
  

    }


    void Start()
    {
        StartCoroutine(BossBehavior());
    }

    IEnumerator BossBehavior()
    {
        // Play the sitting animation
        bossAnimator.SetTrigger("SitTrigger");
        yield return new WaitForSeconds(bossAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Stand up and start walking towards the player if within detection range
        while (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            yield return null;
        }

        bossAnimator.SetTrigger("StandUpTrigger");
        yield return new WaitForSeconds(bossAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Play walking animation and move towards the player
        bossAnimator.SetTrigger("StartWalking");
        bossAnimator.SetBool("IsWalking", true);

        while (Vector3.Distance(transform.position, player.position) > shootingRange)
        {
            navMeshAgent.SetDestination(player.position);
            yield return null;
        }

        // Once in shooting range, stop walking and shoot
        bossAnimator.SetBool("IsWalking", false);

        // Add your shooting logic here, e.g., instantiate bullets, play shooting animation, etc.
        bossAnimator.SetTrigger("FireTrigger");

        yield break;
    }


}
