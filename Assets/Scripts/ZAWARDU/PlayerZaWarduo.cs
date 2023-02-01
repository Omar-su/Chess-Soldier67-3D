using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZaWarduo : MonoBehaviour
{
    private TimeManager timemanager;
    public AudioSource audioSource;
    public AudioClip timeStopsound;
    public AudioClip timeRegain;
    public float volume = 0.5f;
    public GameObject colorTransition;
    public int flashes = 5;
    public float waitTime = 0.04f;    
    public GameObject spotlight1;
    public Color changedColor;
    public Color origColor;
    
    // Start is called before the first frame update
    void Start()
    {
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !timemanager.TimeIsStopped) //Stop Time when Q is pressed
        {
            StartCoroutine("AudioDelay");
            
        }
        if(Input.GetKeyDown(KeyCode.E) && timemanager.TimeIsStopped)  //Continue Time when E is pressed
        {   
            StartCoroutine("TimeRegain");
        }
    }

    IEnumerator TimeRegain(){
        audioSource.Stop();
        audioSource.PlayOneShot(timeRegain, volume);
        yield return new WaitForSeconds(1.4f);
        timemanager.ContinueTime();
        spotlight1.SetActive(false);

    }

    IEnumerator AudioDelay(){
        audioSource.PlayOneShot(timeStopsound, volume);
        spotlight1.SetActive(true);
        Light light = spotlight1.GetComponent<Light>();
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
        yield return new WaitForSeconds(0.5f); 
        audioSource.Play();
    }



}