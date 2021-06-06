using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private Transform parentPlatform;

    private void Start()
    {
        parentPlatform = transform.parent.parent; // tilemap is a grandchild of the platform's transform
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Platform Collision Enter");
        parentPlatform.GetComponent<NPointPlatform>().CollisionEnter(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Platform Collision Exit");
        parentPlatform.GetComponent<NPointPlatform>().CollisionExit(collision);
    }
}
