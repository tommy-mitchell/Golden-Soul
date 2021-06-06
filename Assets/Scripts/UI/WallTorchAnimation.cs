using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTorchAnimation : MonoBehaviour
{
    public float distance;
    public float lightFlickerValue;
    public float waitTime;

    private Light light1;
    private Light light2;

    private int      counter;
    private float    distanceFromPlayer;
    private Animator anim;

    private Transform player;

    private void Start()
    {
        light1 = transform.Find("Spot Light"        ).GetComponent<Light>();
        light2 = transform.Find("Spot Light Reverse").GetComponent<Light>();

        counter            = 0;
        distanceFromPlayer = -1;
        anim               = GetComponent<Animator>();

        player = GameObject.Find("Player").transform;

        Invoke(nameof(StartAnimation), Random.value/2);
        StartAnimation();
    }

    private void Update()
    {
        distanceFromPlayer = Vector2.Distance(transform.position, player.position);

        if(distanceFromPlayer < distance)
        {
            if(!light1.gameObject.activeSelf)
            {
                light1.gameObject.SetActive(true);
                light2.gameObject.SetActive(true);
                anim.enabled = true;
                anim.SetInteger("Random", Random.Range(1, 4 + 1)); // Random.Range(x, y) is in range [x, y)

                StartCoroutine(LightFlicker());
            }

            counter++;

            if(counter % 30 == 0) // update every 30 frames
                anim.SetInteger("Random", Random.Range(1, 4 + 1)); // Random.Range(x, y) is in range [x, y)
        }
        else
        {
            light1.gameObject.SetActive(false);
            light2.gameObject.SetActive(false);
            anim.enabled = false;
        }
    }

    private void StartAnimation()
    {
        anim.SetTrigger("Start");
        StartCoroutine(LightFlicker());
    }

    private IEnumerator LightFlicker()
    {
        while(true)
        {
            if(distanceFromPlayer > distance && distanceFromPlayer != -1)
                yield break;

            ChangeIntensity( lightFlickerValue); // up
            yield return new WaitForSeconds(waitTime);

            if(distanceFromPlayer > distance && distanceFromPlayer != -1)
                yield break;

            ChangeIntensity(-lightFlickerValue); // base
            yield return new WaitForSeconds(waitTime);

            if(distanceFromPlayer > distance && distanceFromPlayer != -1)
                yield break;

            ChangeIntensity(-lightFlickerValue); // down
            yield return new WaitForSeconds(waitTime);

            if(distanceFromPlayer > distance && distanceFromPlayer != -1)
                yield break;

            ChangeIntensity( lightFlickerValue); // base
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void ChangeIntensity(float change)
    {
        light1.intensity += change;
        light2.intensity += change;
    }
}
