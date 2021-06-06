using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Audio Sources", menuName = "Game/Audio/Sword")]
public class SwordAudioSources : ScriptableObject
{
    [SerializeField, Tooltip("The sound that plays when the sword hits something.")]
    private AudioClip _onSwordStick;
    public AudioClip OnSwordStick => _onSwordStick;

    [SerializeField, Tooltip("The sound that plays when the sword is thrown.")]
    private AudioClip _onSwordThrow;
    public AudioClip OnSwordThrow => _onSwordThrow;

    [SerializeField, Tooltip("The sound that plays while attacking.")]
    private AudioClip _onSwordSwing;
    public AudioClip OnSwordSwing => _onSwordSwing;

    [SerializeField, Tooltip("The sound that plays when the sword is recalled.")]
    private AudioClip _onSwordRecall;
    public AudioClip OnSwordRecall => _onSwordRecall;

    [SerializeField, Tooltip("The sound that plays while the sword is travelling through the air.")]
    private AudioClip _swordInAir;
    public AudioClip SwordInAir => _swordInAir;

    [SerializeField, Tooltip("The default sound that plays to notfiy when the sword hits a respawn layer.")]
    private AudioClip _respawnHitSound;
    public AudioClip RespawnHitSound => _respawnHitSound;

    [SerializeField, Tooltip("The sound that plays when the sword lands in water.")]
    private AudioClip _waterHitSound;
    public AudioClip WaterHitSound => _waterHitSound;

    [SerializeField, Tooltip("The sound that plays when the sword collides with spikes.")]
    private AudioClip _spikesHitSound;
    public AudioClip SpikesHitSound => _spikesHitSound;
}