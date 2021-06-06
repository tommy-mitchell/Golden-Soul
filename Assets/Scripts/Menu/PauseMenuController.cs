using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private GameObject player;
    private PlayerStateManager stateManager;
    private PlayerHealth playerHealth;

    public GameObject pauseMenu;
    public bool isPaused;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        stateManager = GameObject.Find("Player State").GetComponent<PlayerStateManager>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            ResumePauseHandler();
    }

    public void ResumePauseHandler()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        TogglePlayerPauseState();
    }

    public void ReturnToCheckPoint()
    {
        //player.transform.position = new Vector3(PlayerPrefs.GetFloat("SaveX"), PlayerPrefs.GetFloat("SaveY"), PlayerPrefs.GetFloat("SaveZ"));
        int currentHealth = stateManager.Health;
        playerHealth.Respawn();
        stateManager.Health = currentHealth;
        Debug.Log("Checkpoint loaded");

        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        TogglePlayerPauseState();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void TogglePlayerPauseState()
    {
        player.GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = !isPaused;
        //player.GetComponent<PlayerController>().enabled = !isPaused;
        //player.GetComponent<PlayerAttack>().enabled = !isPaused;
        //player.GetComponent<PlayerThrow>().enabled = !isPaused;
    }
}
