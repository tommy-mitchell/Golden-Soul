using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollectables : MonoBehaviour
{
    public event Action<string> OnKeyCollect;

    //private Image              keyImage;
    private Text               textCounter;
    private PlayerStateManager stateManager;

    private void Start()
    {
        //keyImage     = GameObject.Find("UI").transform.Find("Keys").Find("Canvas").GetComponentInChildren<Image>();
        textCounter  = GameObject.Find("UI").transform.Find("Keys").Find("Canvas").GetComponentInChildren<Text>();
        stateManager = GameObject.Find("Player State").GetComponent<PlayerStateManager>();
    }

    public void KeyCollect(string keyPathName)
    {
        stateManager.IncrementKeys();
        OnKeyCollect?.Invoke(keyPathName);

        UpdateText();
    }

    public void UpdateText() => textCounter.text = "" + stateManager.NumberOfKeys;

}
