using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryText : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("kittycounter is " + PlayerPrefs.GetInt("KittyCounter"));
        int kittyCounter = PlayerPrefs.GetInt("KittyCounter");
        //GetComponent<TextMeshProUGUI>().SetText(kittyCounter.ToString());
        GetComponent<UnityEngine.UI.Text>().text = kittyCounter.ToString();
    }
}
