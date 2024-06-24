using System.Collections;
using UnityEngine;

public class BombTrackPlayer : MonoBehaviour
{

     [Header("REFERENCES")] 
    [SerializeField] private Rigidbody _rb;
    // [SerializeField] private Target _target;

    [Header("MOVEMENT")] 
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _rotateSpeed = 95;

    [Header("PREDICTION")] 
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")] 
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;

    public HealthSystem healthSystem;

    public float explosionRadius = 5f;
    public float explosionTime = 10f;
    // private Transform player;
    public AudioClip travelSound;
    public ParticleSystem explosionEffectPrefab;
    public AudioClip explosionSound;
    private Transform player;

    public AudioSource travelAudioSource;

    private Rigidbody playerRigidbody;
    private bool hasExploded = false; // New variable to track if the bomb has already exploded
    public TimeManager timemanager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();

        // Get the Rigidbody component attached to the "Player" GameObject
        playerRigidbody = player.GetComponent<Rigidbody>();
        // Set the audio clip to loop
        travelAudioSource.clip = travelSound;
        // travelAudioSource.volume = 0.5f;
        travelAudioSource.loop = true;

        // Start playing the audio clip
        travelAudioSource.Play();


        StartCoroutine(ExplodeAfterTime());
    }

    IEnumerator ExplodeAfterTime()
    {
        yield return new WaitForSeconds(explosionTime);
        Explode();
    }

    private void FixedUpdate() {

         _rb.velocity = transform.forward * _speed;

        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, player.transform.position));

        PredictMovement(leadTimePercentage);

        if(!timemanager.TimeIsStopped){
            AddDeviation(leadTimePercentage);

            RotateRocket();
        }
        

        // Print health to the console
        Debug.Log("Health bomb: " + healthSystem.GetHealth());

        if(healthSystem.GetHealth() <= 0 && !hasExploded)
        {
            hasExploded = true; // Set the flag to true to avoid multiple explosions
            Explode();
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                if(!hasExploded)
                {
                    hasExploded = true; // Set the flag to true to avoid multiple explosions
                    
                    // Instantiate explosion particle effect
                    ParticleSystem explosionInstance = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                    explosionInstance.Play();

                    // Play the explosion sound
                    travelAudioSource.PlayOneShot(explosionSound, 0.5f);



                    // Handle general explosion logic (e.g., shaking the camera, applying force, etc.)
                    // Destroy the effect once it finishes playing
                    Destroy(explosionInstance.gameObject, explosionInstance.main.duration); 

                    Destroy(gameObject, 0.2f);

                }
               
            }
        }
    }

    private void PredictMovement(float leadTimePercentage) {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = playerRigidbody.position + playerRigidbody.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage) {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
        
        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket() {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }


    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                // Handle player explosion logic
            }
        }

        // Instantiate explosion particle effect
        ParticleSystem explosionInstance = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        explosionInstance.Play();

        // Play the explosion sound
        travelAudioSource.PlayOneShot(explosionSound, 0.5f);



        // Handle general explosion logic (e.g., shaking the camera, applying force, etc.)
        // Destroy the effect once it finishes playing
        Destroy(explosionInstance.gameObject, explosionInstance.main.duration); 

        Destroy(gameObject, 0.2f);
    }
}
