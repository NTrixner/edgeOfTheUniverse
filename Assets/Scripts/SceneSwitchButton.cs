using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchButton : MonoBehaviour
{
    
    public void StartGame()
    {
        SceneHandler.INSTANCE.StartGame();
    }

    public void LostGame()
    {
        SceneHandler.INSTANCE.LostGame();
    }

    public void WonGame()
    {
        SceneHandler.INSTANCE.WonGame();
    }

    public void BackToMen()
    {
        SceneHandler.INSTANCE.BackToMen();
    }
}
