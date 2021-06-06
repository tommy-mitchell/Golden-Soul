using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private float _lightFlickerValue = .2f;
    [SerializeField]
    private float _waitTime          = .5f;

    private Light light1;
    private Light light2;

    private void Start()
    {
        light1 = transform.Find("Spot Light"        ).GetComponent<Light>();
        light2 = transform.Find("Spot Light Reverse").GetComponent<Light>();

        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while(true)
        {
            ChangeIntensity( _lightFlickerValue); // up
            yield return new WaitForSeconds(_waitTime);

            ChangeIntensity(-_lightFlickerValue); // base
            yield return new WaitForSeconds(_waitTime);

            ChangeIntensity(-_lightFlickerValue); // down
            yield return new WaitForSeconds(_waitTime);

            ChangeIntensity( _lightFlickerValue); // base
            yield return new WaitForSeconds(_waitTime);
        }
    }

    private void ChangeIntensity(float change)
    {
        light1.intensity += change;
        light2.intensity += change;
    }
}
