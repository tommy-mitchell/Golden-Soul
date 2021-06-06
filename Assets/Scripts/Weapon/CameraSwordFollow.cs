using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwordFollow : MonoBehaviour
{
    public GameObject canvas;
    public GameObject player;
    public       bool forCutscene;

    private Vector3    offset;
    private GameObject sword;

    private void Start()
    {
        if(!forCutscene) GetComponent<Camera>().aspect = 1.5f;

        offset = new Vector3(0, 0, -1);
    }

    void Update()
    {
        if(GameObject.Find("Sword(Clone)"))
        {
            sword = GameObject.Find("Sword(Clone)");

            transform.position = new Vector3(sword.transform.position.x + offset.x, sword.transform.position.y + offset.y, offset.z);
            if(!forCutscene) canvas.SetActive(true);
        }
        else
            if(!forCutscene) canvas.SetActive(false);
    }
}
