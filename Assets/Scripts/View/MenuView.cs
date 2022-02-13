using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace View
{
    public class MenuView : MonoBehaviour
    {
        public static MenuView instance;
        public GameObject panel;
        private void Awake()
        {
            instance = this;
        }
        public void GotoMain()
        {
            MenuManager.instance.GotoMain();
        }
        public void GiveUp()
        {
            // BattleResultView.instance.EnableDeafeatUI();
            Disable();
        }
        public void ExitGame()
        {
            MenuManager.instance.ExitGame();
        }
        public void Enable()
        {
            UIEffect.FadeInPanel(panel);
        }

        public void Disable()
        {
            UIEffect.FadeOutPanel(panel);
        }
    }
}