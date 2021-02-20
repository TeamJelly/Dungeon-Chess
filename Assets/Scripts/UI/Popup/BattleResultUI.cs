using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Battle
{
    public class BattleResultUI : MonoBehaviour
    {
        public GameObject WinUI;
        public GameObject DeafeatUI;
        public static BattleResultUI instance;
        private void Awake()
        {
            instance = this;
        }
        public void EnableWinUI()
        {
            WinUI.SetActive(true);
        }

        public void EnableDeafeatUI()
        {
            DeafeatUI.SetActive(true);
        }
    }
}