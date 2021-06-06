using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRespawn : MonoBehaviour
{
    public GameObject respawnLayer;

    private NPointPlatform controller;

    private void Start()
    {
        controller = GetComponent<NPointPlatform>();
    }

    private void FixedUpdate()
    {
        respawnLayer.SetActive(!controller.IsTouchingPlayer());
    }
}
