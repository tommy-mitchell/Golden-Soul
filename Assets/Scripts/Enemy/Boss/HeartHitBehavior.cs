using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartHitBehavior : MonoBehaviour
{
    public AudioSource hitNoise;
    public GameObject boss;

    [SerializeField]
    private int maxHeartHealth;

    private int heartHealth;

    private Color originalColor;
    private SpriteRenderer heartRenderer;

    private BossControllerBase controller;

    private static float FLASH_TIME = 0.25f;
    //private static Vector3 HEART_POSITION_LEFT = new Vector3(12.37f, 3.49f, 0);
    //private static Vector3 HEART_POSITION_RIGHT = new Vector3(17.39f, 3.49f, 0);
    private void Start()
    {
        heartHealth = maxHeartHealth;
        heartRenderer = GetComponent<SpriteRenderer>();
        originalColor = heartRenderer.color;
        controller = boss.GetComponent<BossControllerBase>();
    }

    private void Update()
    {
        //transform.position = boss.transform.rotation.eulerAngles.y == 180 ? HEART_POSITION_LEFT : HEART_POSITION_RIGHT; 

        if (boss.activeInHierarchy == false)
            gameObject.SetActive(false);

        if(heartHealth <= 0)
        {
            boss.GetComponent<BossHealth>().Damage();
            heartHealth = maxHeartHealth;
        }
    }

    public void HeartHit()
    {
        Debug.Log("hit");
        heartHealth -= 1;
        heartRenderer.color = Color.green;
        hitNoise.Play();

        controller.OnHit();

        Invoke("ResetColor", FLASH_TIME);
    }

    void ResetColor()
    {
        heartRenderer.color = originalColor;
    }
}
