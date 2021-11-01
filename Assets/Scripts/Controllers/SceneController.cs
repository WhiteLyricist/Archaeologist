using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static Action SaveGame = delegate { };
    public static Action LoadGame = delegate { };

    private void Start()
    {
        LoadGame();
        UIController.Menu += OnMenu;
    }

    private void OnApplicationPause(bool pause) //OnApplicationFocuse
    {
        SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void ExitMenu() 
    {
        Parameters.isNewGame = true;
        SceneManager.LoadScene("Menu");
    }

    public void NewGame() 
    {
        Parameters.isNewGame = true;
        SceneManager.LoadScene("Game");
    }

    public void OnLoadGame() 
    {
        SceneManager.LoadScene("Game");
    }

    void OnMenu() 
    {
        Parameters.isNewGame = false;
        SaveGame();
        SceneManager.LoadScene("Menu");
    }

    private void OnDestroy()
    {
        UIController.Menu -= OnMenu;
    }
}
