using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapButtonIndicators : MonoBehaviour
{
    [SerializeField]
    private GameObject _kbmIndicator;

    [SerializeField]
    private GameObject _gamepadIndicator;

    private void Start()
    {
        GameObject.Find("Player").GetComponent<NewInput>().controlsChanged += controlScheme => {
                _kbmIndicator.SetActive(controlScheme == "Keyboard and Mouse");
            _gamepadIndicator.SetActive(controlScheme == "Gamepad");
        };
    }
}
