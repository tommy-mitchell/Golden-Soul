using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public int dmg = 1;

    void OnCollisionEnter2D(Collision2D collision) //Allows hit detection to occur more consistently
    {
        Debug.Log("collision with " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.SendMessage("Damage", dmg);
        }
        else if (collision.gameObject.CompareTag("BossHeart"))
        {
            Debug.Log("Hit boss heart");
            collision.gameObject.GetComponent<HeartHitBehavior>().HeartHit();
        }
    }
}
