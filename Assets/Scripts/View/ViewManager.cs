using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace View
{
    public class ViewManager : MonoBehaviour
    {
        public static ViewManager instance;
        public static BattleView battle;

        [SerializeField]
        private Transform mainPanel;
        public Transform MainPanel => mainPanel;

        private void Awake()
        {
            instance = this;
            battle = GetComponent<BattleView>();
        }
    }
}