﻿using Model;
using Model.Managers;
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
            LoadScene("#1_Field");
        }
        public static void GotoMain()
        {
            LoadScene("#0_Menu");
        }

        public static void GotoPrepareScene()
        {
           LoadScene("#1_Field");
        }
        public static void GotoBattleRoom()
        {
            GameManager.InBattle = true;
            LoadScene("#1_Field");
        }

        public static void GotoTreasureRoom()
        {
             LoadScene("#1_Field");
        }

        public static void GotoShopRoom()
        {
            LoadScene("#1_Field");
        }

        public static void GotoTavernRoom()
        {
           LoadScene("#1_Field");
        }

        public static void GotoEventRoom()
        {
            LoadScene("#1_Field");
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
