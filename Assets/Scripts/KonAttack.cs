using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonAttack : MonoBehaviour
{
    
    private TimeManager timemanager;
    public AudioSource audioSource;
    public AudioClip konSound;
    public float volume = .5f;
    public int flashes = 5;
    public float waitTime = 0.1f;    
    public float waitTimeKon = 0.1f;
    public GameObject spotlight;
    public Color changedColor;
    public Color origColor;
    public Camera fpscamera;
    public float distance = 10f;
    public float radius = 10f;
    public LayerMask layerMask;
    public float damage = 5f;
    public float konForce;
    public float timeBeforeDestruction = .1f;
    private bool isActivated = false;
    public GameObject sphere;
    public Color color = Color.red;
    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H) && !isActivated) {
            isActivated = true;

            StartCoroutine("AttackWithKon");
        }
    }

    IEnumerator AttackWithKon(){
        audioSource.PlayOneShot(konSound, volume);
        StartCoroutine("PlayAnimation");
        spotlight.SetActive(true);
        Light light = spotlight.GetComponent<Light>();
        for(int i = 0; i < flashes; i++) {
            yield return new WaitForSeconds(waitTime);
            light.color = changedColor;
            yield return new WaitForSeconds(waitTime);  
            light.color = origColor;
            yield return new WaitForSeconds(waitTime);
            light.color = changedColor;
            yield return new WaitForSeconds(waitTime);
        }        
        timemanager.StopTime();
        
        Vector3 pos = fpscamera.transform.position + fpscamera.transform.forward * distance; 
        yield return new WaitForSeconds(timeBeforeDestruction);  
        Collider[] colliders = Physics.OverlapSphere(pos, radius, layerMask);
  
        foreach(Collider c in colliders) {
            // c.GetComponent<Rigidbody>().AddForce(konForce * fpscamera.transform.forward, ForceMode.Impulse);
            // Target target = c.GetComponent<Target>();
            // target.TakeDamage(damage);
            Destroy(c.gameObject);
        }
        timemanager.ContinueTime();
        spotlight.SetActive(false);
        isActivated = false;
        anim.ResetTrigger("KonAttack");
        //audioSource.Play(eatSound);
    }

    IEnumerator PlayAnimation(){
        yield return new WaitForSeconds(waitTimeKon);
        anim.SetTrigger("KonAttack");
    }
}
