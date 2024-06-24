using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

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

    public float colorDrift = 0;
    public float scanLineJitter = .02f;
    public float verticalJump = 0;
    public float horizontalShake = 0;
    public GameObject analogGameObj;
    AnalogGlitch analogGlitch;
    // Start is called before the first frame update
    void Start()
    {
        timemanager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        analogGlitch = analogGameObj.GetComponent<AnalogGlitch>();

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

        Glitch(0,0,0,0);
        analogGlitch.enabled = false;

    }

    IEnumerator AudioDelay(){
        audioSource.PlayOneShot(timeStopsound, volume);
        spotlight1.SetActive(true);

        analogGlitch.enabled = true;
        Glitch(colorDrift, scanLineJitter, verticalJump, horizontalShake);


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

    void Glitch(float colorDrift, float scanLineJitter,float verticalJump, float horizontalShake){
        analogGlitch.colorDrift = colorDrift;
        analogGlitch.scanLineJitter = scanLineJitter;
        analogGlitch.verticalJump = verticalJump;
        analogGlitch.horizontalShake = horizontalShake;
    }

}