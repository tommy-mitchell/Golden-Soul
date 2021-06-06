using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowSweepSensor : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GetComponentInParent<BossAttacks>().SetLowSweepState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInParent<BossAttacks>().SetLowSweepState(false);
        }
    }
}
