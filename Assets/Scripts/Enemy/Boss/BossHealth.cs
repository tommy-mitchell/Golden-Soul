using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossHealth : MonoBehaviour
{
    public RectTransform fillRT;
    public AudioSource hitNoise;
    public GameObject victory;
    public GameObject healthBarCanvas;

    
    public float currentHealth = 100;
    public float maxHealth = 100;

    private Color originalColor;
    private SpriteRenderer bossRenderer;

    private static float MAX_FILL = 865.2f;
    private static float FLASH_TIME = 0.25f;

    void Start()
    {
        bossRenderer = GetComponent<SpriteRenderer>();

        originalColor = bossRenderer.color;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            victory.SetActive(true);
            healthBarCanvas.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void Damage()
    {
        currentHealth -= 33.35f;
        fillRT.DOShakePosition(.75f, 20, 30, 45, true, true);
        UpdateFill();

        bossRenderer.color = Color.red;
        hitNoise.Play();

        Invoke("ResetColor", FLASH_TIME);
    }

    public void UpdateFill()
    {
        fillRT.DOSizeDelta(new Vector2((float)currentHealth / maxHealth * MAX_FILL, fillRT.sizeDelta.y), 0.3f);
    }

    void ResetColor()
    {
        bossRenderer.color = originalColor;
    }

    public float GetHealthPercentage()
    {
        return (currentHealth/maxHealth) * 100;
    }
}