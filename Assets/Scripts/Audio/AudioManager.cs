using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private PlayerAudioSources _playerAudio;
    public PlayerAudioSources PlayerAudio { get => _playerAudio; }

    [SerializeField]
    private SwordAudioSources _swordAudio;
    public SwordAudioSources SwordAudio { get => _swordAudio; }

    [SerializeField]
    private EnemyAudioSources _enemyAudio;
    public EnemyAudioSources EnemyAudio { get => _enemyAudio; }

    [SerializeField]
    private EnvironmentAudioSources _environmentAudio;
    public EnvironmentAudioSources EnvironmentAudio { get => _environmentAudio; }
}
