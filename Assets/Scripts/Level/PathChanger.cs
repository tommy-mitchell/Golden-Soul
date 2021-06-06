using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathChanger : MonoBehaviour
{
    private GameObject pathOne;
    private GameObject pathTwo;
    private GameObject pathThree;

    private GameObject start;
    private GameObject finish;

    private bool hasFinished;

    private PathController     controller;
    private PlayerStateManager stateManager;

    private GameObject[] numberToPath = new GameObject[3];

    private void Start()
    {
        pathOne   = transform.Find("1").gameObject;
        pathTwo   = transform.Find("2").gameObject;
        pathThree = transform.Find("3").gameObject;

        numberToPath[0] = pathOne;
        numberToPath[1] = pathTwo;
        numberToPath[2] = pathThree;

        start  = transform.Find( "Start").gameObject;
        finish = transform.Find("Finish").gameObject;

        hasFinished = false;

        controller   = transform.parent.GetComponent<PathController>();
        stateManager = GameObject.Find("Player State").GetComponent<PlayerStateManager>();

        GameObject.Find("Player").GetComponent<PlayerCollectables>().OnKeyCollect += _keyPath => {
            if(_keyPath == transform.name) // only change if key is on this path
            {
                hasFinished = true;
                controller.OnFinish();

                 start.SetActive(false);
                finish.SetActive( true);

                transform.Find((stateManager.NumberOfKeys).ToString()).Find("Grid").Find("Lights").gameObject.SetActive(false);
            }
        };
        controller.OnPathFinish += ChangePath;
    }
    
    public void ChangePath()
    {
        if(!hasFinished)
        {
            //Debug.Log("changing path: " + transform.name);

            for(int index = 0; index < 3; index++)
                numberToPath[index].SetActive(index == stateManager.NumberOfKeys);
        }
    }
}
