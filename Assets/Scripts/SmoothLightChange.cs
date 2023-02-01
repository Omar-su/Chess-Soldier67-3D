using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLightChange : MonoBehaviour
{
    public GameObject spotlightGameObject;
    public Color startColor = Color.white;
    public Color endColor = Color.red;
    public float transitionDuration = 2.0f;

    private Light spotlight;
    private float transitionStartTime;

    private void Start()
    {
        spotlight = spotlightGameObject.GetComponent<Light>();
        if (spotlight == null)
        {
            Debug.LogError("Spotlight game object does not have a Light component.");
            return;
        }

        spotlight.color = startColor;
        transitionStartTime = Time.time;
    }

    void update()
    {
        float elapsedTime = Time.time - transitionStartTime;
        float t = elapsedTime / transitionDuration;
        print(" t value : " + t);

        if (t > 1)
        {
            t = 1;
        }

        spotlight.color = Color.Lerp(startColor, endColor,  Mathf.PingPong(Time.time, 1));

    }
}
