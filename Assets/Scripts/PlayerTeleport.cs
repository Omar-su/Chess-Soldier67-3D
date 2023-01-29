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

    PlayerMovement playerMovement;
    public ParticleSystem teleportEffect;
    public AudioClip zaHando;
    public AudioClip reversedZaHando;
    public float range = 100;
    public Camera fpscamera;
    public float offset = 1f;
    public GameObject objectTeleportEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
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
        print("position in game : " + transform.position+ " y = " + transform.position.y);
    }

    void ShootRayCast(){

        RaycastHit hit;
        
        if (Physics.Raycast(fpscamera.transform.position, fpscamera.transform.forward, out hit, range))
        {
            if (hit.collider.tag == "TeleportableObjects")
            {
                print("name hit "+hit.collider.name);
                StartCoroutine(TeleportObject(hit));
            }
        }
    }

    IEnumerator TeleportObject(RaycastHit hit){
        audioSource.PlayOneShot(reversedZaHando, volume);
        GameObject effect = Instantiate(objectTeleportEffect, hit.point, Quaternion.identity);
        Destroy(effect, 0.5f);        
        yield return new WaitForSeconds(0.1f);
        hit.collider.transform.position = transform.position + ( offset * orientation.forward);
    }

    IEnumerator Teleport(){
        playerMovement.SetDisabled(true);
        teleportEffect.Play();
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
        playerMovement.SetDisabled(false);
    }
}
