using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStateManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSettings _settings;

    [SerializeField]
    private int _health;

    [SerializeField]
    private int _numberOfKeys;

    private void OnValidate()
    {
        Health       = Health;
        NumberOfKeys = NumberOfKeys;
    }

    private void Awake()
    {
        _health       = Settings.MaximumHealthValue;
        _numberOfKeys = 0;
    }

    public PlayerSettings Settings { get => _settings; }

    public int Health
    { 
        get => _health;
        set => _health = Mathf.Clamp(value, 0, Settings.MaximumHealthValue);
    }

    public void IncrementHealth() => Health += 2;

    public int NumberOfKeys
    {
        get => _numberOfKeys;
        private set => _numberOfKeys = Mathf.Clamp(value, 0, 3);
    }

    public void ClearKeys() => NumberOfKeys = 0;

    public void IncrementKeys() => NumberOfKeys++;

    public Vector3 RespawnLocation { get; set; }

    public string CurrentScene { get; set; }

    public void Set(PlayerStateManager newState)
    {
        Health          = newState.Health;
        NumberOfKeys    = newState.NumberOfKeys;
        RespawnLocation = newState.RespawnLocation;
        CurrentScene    = newState.CurrentScene;
    }
}