using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Audio Sources", menuName = "Game/Audio/Enemy")]
public class EnemyAudioSources : ScriptableObject // TODO: rename to enemy_OnDamage
{
    [SerializeField, Tooltip("The sound that plays when a guard takes damage.")]
    private AudioClip _onGuardDamage;
    public AudioClip OnGuardDamage => _onGuardDamage;

    [SerializeField, Tooltip("The sound that plays when a guard dies.")]
    private AudioClip _onGuardDeath;
    public AudioClip OnGuardDeath => _onGuardDeath;


    [SerializeField, Tooltip("The sound that plays when an archer takes damage.")]
    private AudioClip _onArcherDamage;
    public AudioClip OnArcherDamage => _onArcherDamage;

    [SerializeField, Tooltip("The sound that plays when an archer dies.")]
    private AudioClip _onArcherDeath;
    public AudioClip OnArcherDeath => _onArcherDeath;

    [SerializeField, Tooltip("The sound that plays when an archer fires an arrow at the player.")]
    private AudioClip _onArcherArrowFire;
    public AudioClip OnArcherArrowFire => _onArcherArrowFire;


    [SerializeField, Tooltip("The sound that plays when a bat takes damage.")]
    private AudioClip _onBatDamage;
    public AudioClip OnBatDamage => _onBatDamage;

    [SerializeField, Tooltip("The sound that plays when a bat dies.")]
    private AudioClip _onBatDeath;
    public AudioClip OnBatDeath => _onBatDeath;

    [SerializeField, Tooltip("The sound that plays when a bat attacks the player.")]
    private AudioClip _onBatAttack;
    public AudioClip OnBatAttack => _onBatAttack;

    
    // boss stuff
}
