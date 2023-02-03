using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;
public class RewindEffects : MonoBehaviour
{
    public RewindTime rewindObj;
    public int flashes = 5;
    public float waitTime = 0.04f;    
    public float modt = .002f;   
    public float modWaitTime;
    // sound / visual Effects
    public GameObject spotLight;
    public AudioSource audioSource;
    public Color color1;
    public Color color2;
    public GameObject analogGameObj;
    AnalogGlitch analogGlitch;
    
    public float colorDrift = .5f;
    public float scanLineJitter = .03f;
    public float verticalJump = .03f;
    public float horizontalShake = .1f;
    bool isfirstTime;
    // Start is called before the first frame update
    void Start()
    {
        isfirstTime = true;
        analogGlitch = analogGameObj.GetComponent<AnalogGlitch>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(rewindObj == null) {
            // List<GameObject>[] g = GameObject.FindGameObjectsWithTag("TeleportableObjects");
            // if(g.) {
            //     rewindObj = g.GetComponent<RewindTime>();                
            // }
        }else{
            if(rewindObj.isRewinding && isfirstTime){
                audioSource.Play();
                analogGlitch.enabled = true;
                Glitch(colorDrift, scanLineJitter, verticalJump, horizontalShake);
                StartCoroutine("LightEffects");
                isfirstTime = false;
            }else if (!isfirstTime && !rewindObj.isRewinding){
                Glitch(0,0,0,0);
                spotLight.SetActive(false);
                analogGlitch.enabled = false;
                audioSource.Stop();
                isfirstTime = true;
            }
        }

    }


    
    void Glitch(float colorDrift, float scanLineJitter,float verticalJump, float horizontalShake){
        analogGlitch.colorDrift = colorDrift;
        analogGlitch.scanLineJitter = scanLineJitter;
        analogGlitch.verticalJump = verticalJump;
        analogGlitch.horizontalShake = horizontalShake;
    }

    IEnumerator LightEffects(){
        spotLight.SetActive(true);
        modWaitTime = waitTime;
        Light light = spotLight.GetComponent<Light>();
        for(int i = 0; i < flashes; i++) {
            light.color = color1;
            yield return new WaitForSeconds(modWaitTime);
            light.color = color2;
            yield return new WaitForSeconds(modWaitTime);  
            light.color = color1;
            yield return new WaitForSeconds(modWaitTime);
            light.color = color2;
            yield return new WaitForSeconds(modWaitTime);
            modWaitTime = (float) (modWaitTime + modt);
        }
    }

}
