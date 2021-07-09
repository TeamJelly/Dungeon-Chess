using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
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


        public static void GotoLobby()
        {
            LoadScene("Lobby");
        }
        public static void GotoMain()
        {
            LoadScene("#001_MainMenu");
        }

        public static void GotoPrepareScene()
        {
            LoadScene("#002_Preparation");
        }

        public static void GotoStage()
        {
            LoadScene("#003_Stage");
        }

        public static void GotoBattleRoom()
        {
            LoadScene("Lobby");
        }

        public static void GotoTreasureRoom()
        {
            LoadScene("#005_Treasure");
        }

        public static void GotoShopRoom()
        {
            LoadScene("#006_Shop");
        }

        public static void GotoTavernRoom()
        {
            LoadScene("#007_Tavern");
        }

        public static void GotoEventRoom()
        {
            LoadScene("#008_Event");
        }


        public static void LoadRoom(Room.Category category)
        {
            switch (category)
            {
                case Room.Category.Boss:
                case Room.Category.Monster:
                case Room.Category.Elite:
                    GotoBattleRoom();
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
}
