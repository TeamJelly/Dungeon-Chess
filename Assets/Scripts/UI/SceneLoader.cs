using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//추후 수정 예정
public class SceneLoader
{

    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void GotoPrepareScene()
    {
        LoadScene("Game Preparation");
    }
    //Boss, monster, elite일 경우 유형 구분 매개변수 필요할 듯 함.
    public static void GotoBossRoom()
    {
        LoadScene("Boss");
    }
    public static void GotoMonsterRoom()
    {
        LoadScene("SampleScene");
    }
    public static void GotoEliteRoom()
    {
        LoadScene("Elite");
    }
    public static void GotoTreasureRoom()
    {
        LoadScene("Treasure");
    }
    public static void GotoShopRoom()
    {
        LoadScene("Shop");
    }
    public static void GotoTavernRoom()
    {
        LoadScene("Tavern");
    }
    public static void GotoEventRoom()
    {
        LoadScene("Event");
    }

    public static void GotoMain()
    {
        LoadScene("MainMenu");
    }

    public static void GotoStage()
    {
        LoadScene("StageScene");
    }

    public static void LoadRoom(Room.Category category)
    {
        switch (category)
        {
            case Room.Category.Boss:
                GotoBossRoom();
                break;
            case Room.Category.Monster:
                GotoMonsterRoom();
                break;
            case Room.Category.Elite:
                GotoEliteRoom();
                break;
            case Room.Category.Treasure:
                GotoTreasureRoom();
                break;
            case Room.Category.Shop:
                GotoShopRoom();
                break;
            case Room.Category.Tavern:
                GotoTavernRoom();
                break;
            case Room.Category.Event:
                GotoEventRoom();
                break;
            default:
                break;
        }

    }
}
