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
    public int frames = 2;
    // Start is called before the first frame update
    void Start()
    {
        rewindPoints = new ArrayList();
        rb = GetComponent<Rigidbody>();
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

        if(isOrigVelocity) {
            PointsInTime p = (PointsInTime) rewindPoints[rewindPoints.Count - 1];
            origVelocity = p.GetVelocity();
            isOrigVelocity = false;
        }
        if (rewindPoints.Count > 0){
            PointsInTime p = (PointsInTime) rewindPoints[0];
            transform.position = p.GetVector3();
            transform.rotation = p.GetRotation();
            for(int i = 0; i < frames; i++) {
                if (rewindPoints.Count > 0){
                    rewindPoints.RemoveAt(0);
                }
            }
        }else{
            StopRewind();
            rb.velocity = origVelocity;
            isOrigVelocity = true;
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