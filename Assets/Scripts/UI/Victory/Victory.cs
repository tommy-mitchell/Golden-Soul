using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public GameObject cam;
    public AudioSource fanfare;

    void Start()
    {

        StartCoroutine(MusicPlay());

        cam.GetComponent<AudioSource>().Stop();
        fanfare.Play();
    }

    IEnumerator MusicPlay()
    {
        yield return new WaitForSeconds(10);

        GetComponent<AudioSource>().Play();

        yield break;
    }
}
