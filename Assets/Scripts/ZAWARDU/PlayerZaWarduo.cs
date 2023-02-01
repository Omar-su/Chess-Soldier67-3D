using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZaWarduo : MonoBehaviour
{
    private TimeManager timemanager;
    public AudioSource audioSource;
    public AudioClip timeStopsound;
    public AudioClip clockClicking;
    public float volume = .5f;

    public int flashes = 5;
    public float waitTime = 0.04f;    
    public GameObject spotlight1;
    public GameObject spotlight2;
    
    // Start is called before the first frame update
    void Start()
    {
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) //Stop Time when Q is pressed
        {
            StartCoroutine("AudioDelay");
            
        }
        if(Input.GetKeyDown(KeyCode.E) && timemanager.TimeIsStopped)  //Continue Time when E is pressed
        {
            audioSource.Stop();
            timemanager.ContinueTime();
            spotlight1.SetActive(false);
        }
    }

    IEnumerator AudioDelay(){
        audioSource.PlayOneShot(timeStopsound, volume);
        for(int i = 0; i < flashes; i++) {
            spotlight1.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            spotlight1.SetActive(false);
            yield return new WaitForSeconds(waitTime);  
            spotlight2.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            spotlight2.SetActive(false);
            yield return new WaitForSeconds(waitTime);
        }
        spotlight1.SetActive(true);
        timemanager.StopTime();
        yield return new WaitForSeconds(0.9f); 
        audioSource.Play();
    }



}