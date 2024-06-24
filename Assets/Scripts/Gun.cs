using System;
using System.Diagnostics;
using UnityEngine;
using EZCameraShake;
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
    public float volume = 0.5f;

    [SerializeField] private Transform _gunpoint;
    [SerializeField] private GameObject _bulletTrail;

    
    // Update is called once per frame
    void Update ()
    {
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) 
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            audioSource.PlayOneShot(audioSource.clip, volume);
            Shoot();
        }

    }

  void Shoot (){
    muzzleflash.Play();

    //TenShenHanEffect();

    RaycastHit hit;
    if (Physics.Raycast(fpscamera.transform.position, fpscamera.transform.forward, out hit, range))
    {
      //CameraShaker.Instance.ShakeOnce(5f,5f,.5f,.5f);

      Target target = hit.transform.GetComponent<Target>();
      EnemyAiTutorial enemy = hit.transform.GetComponent<EnemyAiTutorial>();
      HealthSystem heathSystem = hit.transform.GetComponent<HealthSystem>();
      if (target != null)
      {
        target.TakeDamage(damage);
        // hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.Impulse);

      } else if(enemy != null) {
        // hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.Impulse);
        enemy.TakeDamage(damage);
      }else if(heathSystem != null) {
        heathSystem.TakeDamage(damage);
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


}
