using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeAiTest : MonoBehaviour
{
    public Transform player;
    public Transform targetPoint; // Assuming you have a target point on your player object

    public float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        if (player)
        {
            targetPoint = player.Find("targetPoint");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player && targetPoint)
        {
            // Direct towards the player
            transform.LookAt(player.position);

            // Move towards the player
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
}
