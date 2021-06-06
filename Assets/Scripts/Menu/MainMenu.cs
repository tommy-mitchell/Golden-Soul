using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuThemeSource;

    //private static float START_POS_X = -5.85f;
    //private static float START_POS_Y = -0.997f;
    //private static float START_POS_Z = 0;

    private void Start()
    {
        Cursor.visible = true;
    }
    IEnumerator Clock()
    {
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGame()
    {
        menuThemeSource.GetComponent<AudioSource>().Pause();
        
        //Resets checkpoint and kitty token data for new game
        /*PlayerPrefs.SetInt("KittyCounter", 0);
        PlayerPrefs.SetString("SceneName", "Sewer");
        PlayerPrefs.SetFloat("SaveX", START_POS_X);
        PlayerPrefs.SetFloat("SaveY", START_POS_Y);
        PlayerPrefs.SetFloat("SaveZ", START_POS_Z);*/

        StartCoroutine(Clock());
    }

    public void LoadGame()
    {
        //SceneManager.LoadScene("Dungeon");
       /* string Checkpoint = PlayerPrefs.GetString("SceneName");
        var loader = SceneManager.LoadSceneAsync(Checkpoint);
        loader.completed += _ =>
        {
            var player = FindObjectOfType<PlayerController>().transform;
            player.position = new Vector3(PlayerPrefs.GetFloat("SaveX"), PlayerPrefs.GetFloat("SaveY"), PlayerPrefs.GetFloat("SaveZ"));
            player.GetComponent<PlayerCollectables>().SetKittyCounter(PlayerPrefs.GetInt("KittyCounter"));
            Debug.Log("Checkpoint loaded");
        };*/
    }


    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
