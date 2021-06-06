using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private SpriteRenderer     playerRenderer;
    private Rigidbody2D        rb;
    private Color              originalColor;
    private Image[]            hearts;
    private Canvas             deathFlash;
    private PlayerStateManager stateManager;
    private PlayerSettings     settings;
    private AudioManager       audioManager;
    //private AudioSource        hitNoise;

    public event Action OnDeath;

    void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        rb             = GetComponent<Rigidbody2D>();
        originalColor  = playerRenderer.color;
        hearts         = GameObject.Find("UI").transform.Find("Hearts").Find("Canvas").GetComponentsInChildren<Image>();
        deathFlash     = GameObject.Find("UI").transform.Find("Death Flash").GetComponent<Canvas>();
        stateManager   = GameObject.Find("Player State").GetComponent<PlayerStateManager>();
        settings       = stateManager.Settings;
        audioManager   = GameObject.Find("Audio").GetComponent<AudioManager>();
        //hitNoise       = audioManager.PlayerAudio.OnPlayerDamage;
    }

    public void UpdateSprites()
    {
        for (int i = 0; i < settings.MaximumHealthValue / 2; i++)
        {
            if ((i + 1) * 2 <= stateManager.Health)
                hearts[i].sprite = fullHeart;
            else if (((i + 1) * 2) - 1  <= stateManager.Health)
                hearts[i].sprite = halfHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    private void OnDamage()
    {
        //if(stateManager.Health > settings.MaximumHealthValue)
        //    stateManager.Health = settings.MaximumHealthValue;

        UpdateSprites();

        if(stateManager.Health <= 0) // -> maybe move to StateManager?
            Respawn();
    }

    public void PlayerDamage(int damage, Vector2 impulse)
    {
        stateManager.Health -= damage;
        playerRenderer.color = Color.red;
        AudioSource.PlayClipAtPoint(audioManager.PlayerAudio.OnPlayerDamage, transform.position);

        rb.AddForce(Vector2.Scale(impulse, new Vector2(1000, 500)));

        Invoke(nameof(ResetColor), settings.DamageFlashTime);

        OnDamage();
    }

    void ResetColor()
    {
        playerRenderer.color = originalColor;
    }

    public void Respawn()
    {
        OnDeath?.Invoke();
        
        stateManager.Health = settings.MaximumHealthValue;
        UpdateSprites();

        StartCoroutine(FlashOnDeath());
    }

    IEnumerator FlashOnDeath()
    {
        //deathFlash.transform.position = transform.position;

        deathFlash.gameObject.SetActive(true);

        yield return new WaitForSeconds(.1f);

        deathFlash.gameObject.SetActive(false);
    }
}