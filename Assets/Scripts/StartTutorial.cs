using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorial : MonoBehaviour
{
    public GameObject Level;
    public GameObject Player;

    private void Start()
    {
        Cursor.visible = false;

        Transform rootManager = GameObject.Find("Root Manager").transform;
            rootManager.Find("Audio"     ).gameObject.SetActive(true);
            rootManager.Find("Game State").gameObject.SetActive(true);

        Level.SetActive(true);
        Player.transform.SetParent(null);
        DontDestroyOnLoad(Player);

        rootManager.Find("UI").gameObject.SetActive(true);
    }
}
