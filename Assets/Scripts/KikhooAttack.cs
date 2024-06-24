using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class KikhooAttack : MonoBehaviour
{
    public Camera fpscamera;
    public float range = 100;
    public float kikhooForce;
    public AudioClip kikohoSound;
    public AudioSource audioSource;
    public float volume = 0.5f;
    public GameObject spotLight;
    public int flashes = 5;
    public float waitTime = 0.04f;    
    public float animationWaitTime = 0.04f;    

    public float distance = 10f;
    public float radius = 10f;
    public LayerMask layerMask;
    public float damage = 5f;
    public Color changedColor;
    public Color origColor;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) {
            Attack();
        }
    }


    void Attack(){
        StartCoroutine("kikhooAttack");
    }

    IEnumerator kikhooAttack(){
        //CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
        StartCoroutine("PlayAnimation");
        // Apply force to objects
        yield return new WaitForSeconds(animationWaitTime);
        Collider[] colliders = Physics.OverlapSphere(fpscamera.transform.position + fpscamera.transform.forward * distance, radius, layerMask);
        foreach(Collider c in colliders) {
            c.GetComponent<Rigidbody>().AddForce(kikhooForce * fpscamera.transform.forward, ForceMode.Impulse);
            Target target = c.GetComponent<Target>();
            EnemyAiTutorial enemy = c.GetComponent<EnemyAiTutorial>();
            if(target != null){
                target.TakeDamage(damage);

            } else if(enemy != null) {
                target.TakeDamage(damage);
            } 
        }
    }


    IEnumerator PlayAnimation(){
        anim.SetTrigger("Kikho");
        yield return new WaitForSeconds(animationWaitTime);
        audioSource.PlayOneShot(kikohoSound, volume);
        spotLight.SetActive(true);
        Light light = spotLight.GetComponent<Light>();
        for(int i = 0; i < flashes; i++) {
            spotLight.GetComponent<Light>().color = origColor;
            yield return new WaitForSeconds(waitTime);
            spotLight.GetComponent<Light>().color = changedColor;
            yield return new WaitForSeconds(waitTime);   
        }
        yield return new WaitForSeconds(waitTime);
        spotLight.SetActive(false);
        anim.ResetTrigger("Kikho");
    }
    
}
