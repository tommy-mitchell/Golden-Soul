using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVictory : MonoBehaviour
{
    public Canvas victoryCanvas;

    private void OnEnable()
    {
        victoryCanvas.enabled = false;
    }

    private void OnDisable()
    {
        victoryCanvas.enabled = true;
    }
}
