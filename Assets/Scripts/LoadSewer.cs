using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSewer : MonoBehaviour
{
    private void Start() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}
