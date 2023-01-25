using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform orientation;
    public float teleportRange;

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
    }

    IEnumerator Teleport(){
        playerMovement.SetDisabled(true);
        yield return new WaitForSeconds(0.1f);

        // Teleports the player
        var newpos = orientation.forward * teleportRange;
        newpos.y = 0;
        transform.position += newpos;

        yield return new WaitForSeconds(0.1f);
        playerMovement.SetDisabled(false);
    }
}
