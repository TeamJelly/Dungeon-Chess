using System.Collections;
using System.Collections.Generic;
using UI.Popup;
using UnityEngine;

namespace Model.Managers
{
    public class TreasureManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            BattleResultUI.instance.EnableWinUI();
        }
    }
}