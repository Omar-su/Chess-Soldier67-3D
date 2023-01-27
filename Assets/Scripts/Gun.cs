using System;
using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour{


    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public GameObject flashLight;
    public Camera fpscamera;
    public ParticleSystem muzzleflash;
    public GameObject impactEffect;
    public AudioSource audioSource;
    public float flashTime;

    private float nextTimeToFire = 0f;

    [SerializeField] private Transform _gunpoint;
    [SerializeField] private GameObject _bulletTrail;

    
    // Update is called once per frame
    void Update ()
    {
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) 
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            audioSource.PlayOneShot(audioSource.clip);
            Shoot();
        }

    }

    void Shoot (){
    // muzzleflash.Play();

    //TenShenHanEffect();

    RaycastHit hit;
    if (Physics.Raycast(fpscamera.transform.position, fpscamera.transform.forward, out hit, range))
    {
      UnityEngine.Debug.Log(hit.transform.name);

      Target target = hit.transform.GetComponent<Target>();
      if (target != null)
      {
        target.TakeDamage(damage);
        hit.rigidbody.AddForce(-hit.normal * impactForce);

      }

    }

    TrailEffect(hit);

  }

  private void TrailEffect(RaycastHit hit)
  {
    var trail = Instantiate(
        _bulletTrail,
        _gunpoint.position,
        transform.rotation
    );

    var trailScript = trail.GetComponent<BulletTrail>();

    trailScript.SetTargetPosition(hit.point);
    
    GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
    Destroy(impactGO, 0.5f);
  }

  private void TenShenHanEffect()
  {
    flashLight.SetActive(true);
    StartCoroutine(Flash());
  }


  //after same sec Object to false
  IEnumerator Flash()
    {
        yield return new WaitForSeconds(flashTime);
        flashLight.SetActive(false);
        print("is false");
    }
}
