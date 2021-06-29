using Model;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Model.Managers
{
    public class GameManager
    {
        static GameManager instance;

        public Vector2 ScrollRectTransformPosition;

        public List<Vector2Int> roomHistory = new List<Vector2Int>();
        public Room currentRoom = null;
        public Room[,] AllRooms = null;
        public List<Vector2Int>[] pathList = null;
        List<Unit> partyUnits = new List<Unit>();
        Unit leader;
        public int Gold
        {
            get => gold;
            set
            {
                gold = value;
                HUD.instance.UpdateUI();
            }
        }
        private int gold;
        public int stage;
        public static List<Unit> PartyUnits { get => Instance.partyUnits; }

        public static Unit LeaderUnit { get => instance.leader; set => instance.leader = value; }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        GameManager()
        {
            //InitForTesting();
        }

        public static void Reset()
        {
            instance = new GameManager();
            UnitManager.Reset();
        }

        public static void AddPartyUnit(Unit unit)
        {
            PartyUnits.Add(unit);
            unit.Category = Category.Party;
        }

        public static void RemovePartyUnit(Unit unit)
        {
            PartyUnits.Remove(unit);
        }
    }
}