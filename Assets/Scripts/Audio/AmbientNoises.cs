using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientNoises : MonoBehaviour
{
    [SerializeField, Tooltip("Min. wait time between ambient noises, in seconds.")]
    private int _minimumWaitTime = 20;
    [SerializeField, Tooltip("Max. wait time between ambient noises, in seconds.")]
    private int _maximumWaitTime = 70;

    private EnvironmentAudioSources environmentAudio;

    private void Start()
    {
        environmentAudio = GameObject.Find("Audio").GetComponent<AudioManager>().EnvironmentAudio;

        StartCoroutine(PlayAmbientNoise());
    }

    private IEnumerator PlayAmbientNoise()
    {
        while(true)
        {
            float waitTime = Random.Range(_minimumWaitTime, _maximumWaitTime + 1);

            yield return new WaitForSecondsRealtime(waitTime);

            AudioSource.PlayClipAtPoint(environmentAudio.AmbientNoise, transform.position, Random.value + .5f); // play random clip w/ random volume (minimum of .5 volume)
        }
    }
}
