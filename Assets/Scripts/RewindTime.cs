using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class RewindTime : MonoBehaviour
{
    public bool isRewinding;
    public float timeToRewind = 5f;
    ArrayList rewindPoints;
    Vector3 origVelocity;
    bool isOrigVelocity = true;
    Rigidbody rb;
    public int flashes = 5;
    public float waitTime = 0.04f;    
    public float modt = .002f;   
    public float modWaitTime;
    // sound / visual Effects
    public GameObject spotLight;
    AudioSource audioSource;
    public Color color1;
    public Color color2;
    public AnalogGlitch analogGlitch;
    public float colorDrift = .5f;
    public float scanLineJitter = .03f;
    public float verticalJump = .03f;
    public float horizontalShake = .1f;
    // Start is called before the first frame update
    void Start()
    {
        rewindPoints = new ArrayList();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            StartRewind();
        } else if(Input.GetKeyDown(KeyCode.Y)) {
            StopRewind();
        }
    }

    void StartRewind(){
        isRewinding = true;
        rb.isKinematic = true;
    }

    void StopRewind(){
        isRewinding = false;
        rb.isKinematic = false;
        spotLight.SetActive(false);
    }

    void FixedUpdate() {
        if(isRewinding){
            Rewind();
        }else{
            RecordPoints();
        }
        
    }

    void RecordPoints(){
        if (rewindPoints.Count > Mathf.Round(timeToRewind / Time.fixedDeltaTime)){
			rewindPoints.RemoveAt(rewindPoints.Count - 1);
		}
        PointsInTime p = new PointsInTime(transform.position, transform.rotation, rb.velocity);
        rewindPoints.Insert(0,p);
    }

    void Rewind(){

        Glitch(colorDrift, scanLineJitter, verticalJump, horizontalShake);
        if(isOrigVelocity) {
            PointsInTime p = (PointsInTime) rewindPoints[rewindPoints.Count - 1];
            origVelocity = p.GetVelocity();
            isOrigVelocity = false;
            StartCoroutine("LightEffects");
        }
        if (rewindPoints.Count > 0){
            PointsInTime p = (PointsInTime) rewindPoints[0];
            transform.position = p.GetVector3();
            transform.rotation = p.GetRotation();
            rewindPoints.RemoveAt(0);
        }else{
            StopRewind();
            rb.velocity = origVelocity;
            isOrigVelocity = true;
            Glitch(0,0,0,0);
            audioSource.Stop();
        }
    
    }

    void Glitch(float colorDrift, float scanLineJitter,float verticalJump, float horizontalShake){
        analogGlitch.colorDrift = colorDrift;
        analogGlitch.scanLineJitter = scanLineJitter;
        analogGlitch.verticalJump = verticalJump;
        analogGlitch.horizontalShake = horizontalShake;
    }

    IEnumerator LightEffects(){
        audioSource.Play();
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

struct PointsInTime {
    Vector3 position;
    Quaternion rotation;
    Vector3 velocity;

    public PointsInTime(Vector3 _pos, Quaternion _rot, Vector3 _velocity ){
        this.position = _pos;
        this.rotation = _rot;
        this.velocity = _velocity;
    }

    public Vector3 GetVector3(){
        return position;
    }
    public Quaternion GetRotation(){
        return rotation;
    }
    public Vector3 GetVelocity(){
        return velocity;
    }
}