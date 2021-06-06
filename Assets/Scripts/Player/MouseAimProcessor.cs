using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif

public class MouseAimProcessor : InputProcessor<Vector2>
{
    #if UNITY_EDITOR
    static MouseAimProcessor()
    {
        Initialize();
    }
    #endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<MouseAimProcessor>();
    }

    // normalizes mouse position based on player position to act like a gamepad stick value -> each axis [-1, 1]
    public override Vector2 Process(Vector2 value, InputControl control)
    {
        Vector2 centerPos = new Vector2(Screen.width / 2, Screen.height / 2);

     //   Debug.Log("       aim: " +  value);
     //   Debug.Log("    center: " + (value - centerPos));
     //   Debug.Log("normalized: " + (value - centerPos).normalized);

        return (value - centerPos).normalized;
    }
}