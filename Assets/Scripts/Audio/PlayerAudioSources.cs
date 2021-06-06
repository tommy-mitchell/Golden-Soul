using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Audio Sources", menuName = "Game/Audio/Player")]
public class PlayerAudioSources : ScriptableObject
{
    [SerializeField, Tooltip("The sound that plays when the player takes damage.")]
    private AudioClip _onPlayerDamage;
    public AudioClip OnPlayerDamage => _onPlayerDamage;

    [SerializeField, Tooltip("The sound that plays when the player is walking.")]
    private AudioClip _footsteps;
    public AudioClip Footsteps => _footsteps;
}
