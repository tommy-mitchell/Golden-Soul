using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyHealth : MonoBehaviour // very similar to player health -> one Health script they inherit from?
{
    //public AudioSource hitReceived;
    //public GameObject lootDrop;

    public int maxHealth;
    private int enemyHealth;

    private Vector2 originalPos;
    private SpriteRenderer myRenderer;
    private Color originalColor;
    private EnemyController controller;

    private static float FLASH_TIME = 0.25f;
    
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<EnemyController>();

        enemyHealth = maxHealth;
        originalPos = transform.position;
        originalColor = myRenderer.color;
    }

    private void Respawn()
    {
        if (GetComponent<DOTweenPath>() != null)
        {
            Tween myTween = GetComponent<DOTweenPath>().GetTween();
            enemyHealth = maxHealth;
            myTween.Restart();
        }

        enemyHealth = maxHealth;
        transform.position = originalPos;
        gameObject.SetActive(true);
    }

    public void Damage(int damage)
    {
        enemyHealth -= damage;
        myRenderer.color = Color.red;
        
        //hitReceived.pitch = Random.Range(1f, 1.1f);
        AudioSource.PlayClipAtPoint(controller.OnHitSound, transform.position, .3f);

        controller.OnHit();

        Invoke(nameof(ResetColor), FLASH_TIME);

        //Meant to implement droprates of items
        //if(enemyHealth <= 0) 
        //{
        //    if (Random.Range(0f, 1f) >= 0.5f && lootDrop != null)
        //        Instantiate(lootDrop, transform);
        //}

        if(enemyHealth <= 0)
        {
            gameObject.SetActive(false);
            //Invoke("Respawn", 2);
        }
    }

    void ResetColor() => myRenderer.color = originalColor;

    public int GetHealth() => enemyHealth;
}
