using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public string scene;
    public int    keysInLevel;     
    //public AudioSource doorNoise;

    private Animator     transitionAnimator;
    private Animator     anim;
    private bool         inDoor;
    private GameObject   doorCanvas;
    private AudioManager audioManager;
    private PlayerStateManager stateManager;

    private void Start()
    {
        transitionAnimator = GameObject.Find("Scene Transition").transform.Find("Panel").GetComponent<Animator>();
        anim         = GetComponent<Animator>();
        doorCanvas   = transform.Find("Door Exit").gameObject;
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        stateManager = GameObject.Find("Player State").GetComponent<PlayerStateManager>();

        doorCanvas.SetActive(false);
        inDoor = false;
    }

    private void Update()
    {
        if(stateManager.NumberOfKeys == keysInLevel && inDoor && Input.GetKeyDown(KeyCode.S))
            OpenDoor();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(stateManager.NumberOfKeys == keysInLevel)
        {
            doorCanvas.SetActive(true);
            inDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        doorCanvas.SetActive(false);
        inDoor = false;
    }

    private void SceneLoad()
    {
        SceneManager.LoadScene(scene);
    }

    private void OpenDoor()
    {
        AudioSource.PlayClipAtPoint(audioManager.EnvironmentAudio.OnDoorOpen, transform.position);
        anim.SetBool("Is Open", true);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnimator.SetTrigger("end");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(scene);

        /*if(stateManager.NumberOfKeys != 3) // only change with tutorial
        {
            stateManager.ClearKeys();
            GameObject.Find("Player").GetComponent<PlayerCollectables>().UpdateText();
            GameObject.Find("Player").transform.position = new Vector2(-5f, 0);
        }*/
    }
}
