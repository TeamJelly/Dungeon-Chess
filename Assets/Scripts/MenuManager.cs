using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.LoadScene("Game Preparation");
    }

    public void GotoMainMenu()
    {
        SceneLoader.LoadScene("MainMenu");
    }

    public void GotoStageScene()
    {
        SceneLoader.LoadScene("StageScene");
    }
}
