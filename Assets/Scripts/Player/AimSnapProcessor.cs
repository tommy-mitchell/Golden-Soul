using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif

public class AimSnapProcessor : InputProcessor<Vector2>
{
    #if UNITY_EDITOR
    static AimSnapProcessor()
    {
        Initialize();
    }
    #endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<AimSnapProcessor>();
    }

    [Tooltip("The minimum y-value necessary for the raw aim input to snap to straight up/down.")]
    public float snapValueY = .95f;

    [Tooltip("The minimum x-value necessary for the raw aim input to snap to straight left/right.")]
    public float snapValueX = .99f;

    // snaps the aim value to the cardinal directions when near
    public override Vector2 Process(Vector2 value, InputControl control)
    {
        if(Mathf.Abs(value.y) >= snapValueY)
        {
            if(value.y > 0)
                return value.x > 0 ? Vector2.up   : new Vector2(-.0001f,  1); // maybe put constants like -.0001f into a SO
            else
                return value.x > 0 ? Vector2.down : new Vector2(-.0001f, -1);
        }
        else if(Mathf.Abs(value.x) >= snapValueX)
            return value.x > 0 ? Vector2.right : Vector2.left;
        else
            return value;
    }
}