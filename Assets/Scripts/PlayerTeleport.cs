using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform orientation;
    public float teleportRange;
    public AudioSource audioSource;
    public float volume = 0.5f;
    public MoveCamera moveCamera; 
    public Transform playerCamBoogieWoogie; 
    

    NewMovement playerMovement;
    public ParticleSystem teleportEffect;
    public AudioClip zaHando;
    public AudioClip reversedZaHando;
    public float range = 100;
    public Camera fpscamera;
    public float offset = 1f;
    public GameObject objectTeleportEffect;
    public GameObject spotLight;
    public GameObject spotLight2;
    public Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = gameObject.GetComponent<NewMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) {
            StartCoroutine("Teleport");
            
        } 
        if(Input.GetKeyDown(KeyCode.G)){
            ShootRayCast();
        }
        if(Input.GetKeyDown(KeyCode.O)){
            BoogieWoogie();
        }
    }

    void BoogieWoogie(){
        RaycastHit hit;
        
        if (Physics.Raycast(fpscamera.transform.position, fpscamera.transform.forward, out hit, range))
        {
            if (hit.collider.tag == "TeleportableObjects")
            {
                anim.SetTrigger("ComeOverHere");
                StartCoroutine(Switch(hit));
            }
        }
    }


    IEnumerator Switch(RaycastHit hit){
        spotLight2.SetActive(true);
        audioSource.PlayOneShot(reversedZaHando, volume);
        GameObject effect = Instantiate(objectTeleportEffect, hit.point, Quaternion.identity);
        Destroy(effect, 0.5f);        
        yield return new WaitForSeconds(0.1f);
        spotLight2.SetActive(false);
        Vector3 objetCurrrentPos = hit.collider.transform.position;
        hit.collider.transform.position = transform.position;
        transform.position = objetCurrrentPos;
        playerCamBoogieWoogie.rotation = Quaternion.Euler(playerCamBoogieWoogie.rotation.x, playerCamBoogieWoogie.rotation.y + 180, playerCamBoogieWoogie.rotation.z);

        anim.ResetTrigger("ComeOverHere");
    }


    void ShootRayCast(){

        RaycastHit hit;
        
        if (Physics.Raycast(fpscamera.transform.position, fpscamera.transform.forward, out hit, range))
        {
            if (hit.collider.tag == "TeleportableObjects")
            {
                anim.SetTrigger("ComeOverHere");
                StartCoroutine(TeleportObject(hit));
            }
        }
    }

    IEnumerator TeleportObject(RaycastHit hit){
        spotLight2.SetActive(true);
        audioSource.PlayOneShot(reversedZaHando, volume);
        GameObject effect = Instantiate(objectTeleportEffect, hit.point, Quaternion.identity);
        Destroy(effect, 0.5f);        
        yield return new WaitForSeconds(0.1f);
        spotLight2.SetActive(false);
        hit.collider.transform.position = transform.position + ( offset * orientation.forward);
        anim.ResetTrigger("ComeOverHere");
    }

    IEnumerator Teleport(){
        playerMovement.SetDisabled(true);
        teleportEffect.Play();
        spotLight.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        // Teleports the player
        var newpos = orientation.forward * teleportRange;
        newpos.y = 0;
        
        // Audio
        audioSource.PlayOneShot(zaHando, volume);

        moveCamera.ChangeCameraPosition(newpos);
        transform.position += newpos;
        print("position : " + newpos + " y = " + newpos.y);
        yield return new WaitForSeconds(0.1f);
        spotLight.SetActive(false);
        playerMovement.SetDisabled(false);
    }
}
