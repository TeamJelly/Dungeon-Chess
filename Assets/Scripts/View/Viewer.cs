using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace View
{
    public class Viewer : MonoBehaviour
    {
        public static Viewer instance;
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