using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Environment Audio Sources", menuName = "Game/Audio/Environment")]
public class EnvironmentAudioSources : ScriptableObject
{
    [SerializeField, Tooltip("The sound that plays when a door is opened.")]
    private AudioClip _onDoorOpen;
    public AudioClip OnDoorOpen => _onDoorOpen;

    [SerializeField, Tooltip("The sound that plays when a heart collectable is picked up.")]
    private AudioClip _onHeartPickup;
    public AudioClip OnHeartPickup => _onHeartPickup;

    [SerializeField, Tooltip("The sound that plays when a key collectable is picked up.")]
    private AudioClip _onKeyPickup;
    public AudioClip OnKeyPickup => _onKeyPickup;

    [SerializeField, Tooltip("Ambient noises.")]
    private List<AudioClip> _ambientNoises;
    public AudioClip AmbientNoise => _ambientNoises[Random.Range(0, _ambientNoises.Count)]; // returns a random ambient clip
}
