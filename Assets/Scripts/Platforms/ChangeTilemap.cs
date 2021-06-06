using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTilemap : MonoBehaviour
{
    public Transform from;
    public Transform to;

    public bool returnOnStop;
    public bool switchAtEnd;

    private bool canSwitch;

    private NPointPlatform platform;

    private void Start()
    {
        canSwitch = true;

        platform = GetComponent<NPointPlatform>();
    }

    private void FixedUpdate() // change to isMoving?
    {
        if(canSwitch)
        {
            if(switchAtEnd && platform.HasStopped())
            {
                Switch(true);

                canSwitch = false;
            }
            else if(!switchAtEnd)
            {
                if(platform.IsTouchingPlayer() && !platform.HasStopped())
                {
                    Switch(true);

                    canSwitch = returnOnStop;
                }
                else if(returnOnStop && platform.HasStopped())
                {
                    Switch(false);

                    canSwitch = false;
                }
            }
        }
    }

    private void Switch(bool forward)
    {
        SwapSword();

        from.gameObject.SetActive(!forward);
          to.gameObject.SetActive( forward);
    }

    private void SwapSword()
    {
        if(from.Find("Sword(Clone)"))
            from.Find("Sword(Clone)").SetParent(to);
        else if(to.Find("Sword(Clone)"))
            to.Find("Sword(Clone)").SetParent(from);
    }
}
