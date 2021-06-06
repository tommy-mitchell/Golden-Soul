using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    //public AudioSource pickUpSound;

    private AudioManager       audioManager;
    private PlayerStateManager stateManager;
    private PlayerSettings     settings;
    private PlayerHealth       player;

    [SerializeField]
    private bool _forTutorial = false;

    private void Start()
    {
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        stateManager = GameObject.Find("Player State").GetComponent<PlayerStateManager>();
        settings     = stateManager.Settings;
        player       = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(gameObject.CompareTag("Heart"))
        {
            if(stateManager.Health < settings.MaximumHealthValue)
            {
                stateManager.IncrementHealth();
                player.UpdateSprites(); // temp fix to update health UI on collection
                Collect();
            }
        }
        else if(gameObject.CompareTag("Key"))
        {
            string pathName = _forTutorial ? "" : transform.parent.parent.parent.parent.parent.name; // key is five layers deep
            stateManager.Health = settings.MaximumHealthValue;
            player.UpdateSprites();
            col.gameObject.GetComponent<PlayerCollectables>().KeyCollect(pathName);
            Collect();
        }
    }

    private void Collect()
    {
        AudioClip pickupSound = gameObject.CompareTag("Heart") ? audioManager.EnvironmentAudio.OnHeartPickup : audioManager.EnvironmentAudio.OnKeyPickup;

        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        gameObject.SetActive(false);
    }
}
