using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler INSTANCE;

    [SerializeField] private int MainMenuScene;
    [SerializeField] private int GameScene;
    [SerializeField] private int GameLostScene;
    [SerializeField] private int GameWonScene;


    public long DaysPassed;
    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
            return;
        }

        INSTANCE = this;
        DontDestroyOnLoad(this);
        DaysPassed = 0;
    }

    public void StartGame()
    {
        Debug.Log("Start Game called");
        SceneManager.LoadScene(GameScene);
    }

    public void LostGame()
    {
        Debug.Log("Lost Game called");
        SceneManager.LoadScene(GameLostScene);
    }

    public void WonGame()
    {
        Debug.Log("Won Game called");
        SceneManager.LoadScene(GameWonScene);
    }

    public void BackToMen()
    {
        Debug.Log("Back to Menu called");
        SceneManager.LoadScene(MainMenuScene);
    }
}
