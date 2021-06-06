using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public BossControllerBase controller;

    private bool started = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!started && col.gameObject.tag == "Player")
        {
            controller.BeginFight();
            started = true;
        }  
    }
}
