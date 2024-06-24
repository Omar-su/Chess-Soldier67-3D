using System.Collections;
using UnityEngine;

public class BombTracker : MonoBehaviour
{
    private Transform player;
    public Transform firePoint;
    public GameObject bombPrefab;
    public int nrBombs = 10;

    public AudioClip releaseSound;


    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ShootBombs());
        }
    }

    IEnumerator ShootBombs()
    {
        for (int i = 0; i < nrBombs; i++)
        {
            // Calculate a rotation angle based on the bomb's index
            float angle = i * (360f / 10f);
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

            // Instantiate the bomb with the calculated rotation
            GameObject bomb = Instantiate(bombPrefab, firePoint.position, rotation);

            // Play release sound
            if (audioSource != null && releaseSound != null)
            {
                audioSource.PlayOneShot(releaseSound);
            }



            yield return new WaitForSeconds(0.5f);
        }
    }
}
