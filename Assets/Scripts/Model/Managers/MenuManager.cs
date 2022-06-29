using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Model.Managers
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;
        private void Awake()
        {
            instance = this;
        }
        public void GotoMain()
        {
            GameManager.Reset();
            SceneLoader.GotoMain();
        }
        public void GotoLobby()
        {
            SceneLoader.GotoLobby();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}