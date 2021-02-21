using Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  UI.Popup
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
            UIEffect.FadeInPanel(WinUI);
        }

        public void EnableDeafeatUI()
        {
            UIEffect.FadeInPanel(DeafeatUI);
        }
    }
}