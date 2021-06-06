using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
//   * save enemy states
//   * load game on respawn
//   * path updating on respawn/load
//   * maybe save locations of moving paths? -> or at least ones that are in motion

public class SaveAndLoad : MonoBehaviour
{
    private string FILE_PATH;

    private void Awake()
    {
        FILE_PATH = Application.persistentDataPath + "/save.gs"; // add date/time?
    }

    public void SaveGame()
    {
        PlayerStateManager currentState = GetComponentInChildren<PlayerStateManager>(); // get copy of current game state

        BinaryFormatter bf   = new BinaryFormatter();
        FileStream      file = File.Create(FILE_PATH); // create game save

        bf.Serialize(file, currentState); // save game to file
        file.Close();

        Debug.Log("Game saved.");
    }

    public void LoadGame()
    {
        if(File.Exists(FILE_PATH))
        {
            BinaryFormatter bf   = new BinaryFormatter();
            FileStream      file = File.Open(FILE_PATH, FileMode.Open); // get game save

            PlayerStateManager savedState = (PlayerStateManager) bf.Deserialize(file); // read game save
            file.Close();

            GetComponent<PlayerStateManager>().Set(savedState);

            Debug.Log("Game loaded.");
        }
        else
            Debug.Log("No save file found.");
    }
}
