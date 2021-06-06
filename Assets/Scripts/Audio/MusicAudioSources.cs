using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Music Audio Sources", menuName = "Game/Audio/Music")]
public class MusicAudioSources : ScriptableObject
{
    [SerializeField, Tooltip("The music that plays in the background of the sewer level.")]
    private AudioClip _sewerMusic;
    public AudioClip SewerMusic => _sewerMusic;

    [SerializeField, Tooltip("The music that plays in the background of the dungeon level.")]
    private AudioClip _dungeonMusic;
    public AudioClip DungeonMusic => _dungeonMusic;

    [SerializeField, Tooltip("The music that plays in the background during the boss fight.")]
    private AudioClip _bossMusic;
    public AudioClip BossMusic => _bossMusic;

    [SerializeField, Tooltip("The music that plays after beating the game.")]
    private AudioClip _victoryMusic;
    public AudioClip VictoryMusic => _victoryMusic;

    /*public Dictionary<string, AudioClip> MusicClips = new Dictionary<string, AudioClip>() {
        {   "Sewer", _sewerMusic   },
        { "Dungeon", _dungeonMusic }
    };*/
}
