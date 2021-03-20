using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class Viewer : MonoBehaviour
    {
        public static BattleView battle;

        private void Awake()
        {
            battle = GetComponent<BattleView>();
        }
    }
}