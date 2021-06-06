using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardWeapon : MonoBehaviour
{
    private GuardController controller;

    private void Start()
    {
        controller = GetComponentInParent<GuardController>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Player")
        {
            col.gameObject.GetComponent<PlayerHealth>().PlayerDamage(1, Vector2.zero);

            controller.OnAttackHit();
        }
    }
}
