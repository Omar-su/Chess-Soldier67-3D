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
    public float flashTime;

    private float nextTimeToFire = 0f;

    // Update is called once per frame
    void Update ()
    {
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) 
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            print("shot");
            Shoot();
        }

    }

    void Shoot ()
    {
        // muzzleflash.Play();

        flashLight.SetActive(true);
        print("isactive");
        StartCoroutine(Flash());

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

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.5f);
        }

    }


    //after same sec Object to false
    IEnumerator Flash()
    {
        yield return new WaitForSeconds(flashTime);
        flashLight.SetActive(false);
        print("is false");
    }
}
