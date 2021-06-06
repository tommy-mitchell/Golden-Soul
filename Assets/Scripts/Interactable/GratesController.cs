using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GratesController : MonoBehaviour
{
    void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Sword"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Arrow"), true);
    }
}
