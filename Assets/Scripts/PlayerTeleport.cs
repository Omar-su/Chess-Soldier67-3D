using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform orientation;
    public float teleportRange;
    public AudioSource audioSource;
    public float volume = 0.5f;

    PlayerMovement playerMovement;
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
        print("position in game : " + transform.position+ " y = " + transform.position.y);
    }

    IEnumerator Teleport(){
        playerMovement.SetDisabled(true);
        yield return new WaitForSeconds(0.2f);

        // Teleports the player
        var newpos = orientation.forward * teleportRange;
        newpos.y = 0;
        audioSource.PlayOneShot(audioSource.clip, volume);
        transform.position += newpos;
        print("position : " + newpos + " y = " + newpos.y);
        yield return new WaitForSeconds(0.1f);
        playerMovement.SetDisabled(false);
    }
}
