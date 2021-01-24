using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void StartGame()
    {
        SceneLoader.GotoPrepareScene();
    }

    public void GotoMain()
    {
        SceneLoader.GotoMain();
    }

    public void GotoStage()
    {
        SceneLoader.GotoStage();
    }

}
