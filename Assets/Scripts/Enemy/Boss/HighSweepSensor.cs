﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighSweepSensor : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GetComponentInParent<BossAttacks>().SetHighSweepState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInParent<BossAttacks>().SetHighSweepState(false);
        }
    }
}
