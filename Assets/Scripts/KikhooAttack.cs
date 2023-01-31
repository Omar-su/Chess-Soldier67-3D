using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) {
            kikhooAttack();
        }
    }


    void kikhooAttack(){

        RaycastHit hit;
        
        if (Physics.Raycast(fpscamera.transform.position, fpscamera.transform.forward, out hit, range))
        {
            if (hit.collider.tag == "TeleportableObjects")
            {
                StartCoroutine(kikhooAttack(hit));
            
            }
        }
    }

    IEnumerator kikhooAttack(RaycastHit hit){
        spotLight.SetActive(true);
        yield return new WaitForSeconds(0.08f);
        audioSource.PlayOneShot(kikohoSound, volume);
        hit.transform.GetComponent<Rigidbody>().AddForce(kikhooForce * fpscamera.transform.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(0.04f);
        spotLight.SetActive(false);
    }
}
