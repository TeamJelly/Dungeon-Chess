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
        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        public Vector2 ScrollRectTransformPosition;
        public List<Vector2Int> roomHistory = new List<Vector2Int>();
        public Room currentRoom = null;
        public Room[,] AllRooms = null;
        public List<Vector2Int>[] pathList = null;
        public List<Unit> partyUnits = new List<Unit>();
        public List<Item> itemBag = new List<Item>();
        public List<Artifact> artifactBag = new List<Artifact>();

        private int gold;
        public int stage;


        bool inBattle = false;
        public int Gold
        {
            get => gold;
            set
            {
                gold = value;

                // View.HUDView.instance.UpdateUI();
            }
        }
        public static List<Unit> PartyUnits { get => Instance.partyUnits; }


        public static bool InBattle { get => Instance.inBattle; set => Instance.inBattle = value; }

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
            if (PartyUnits.Contains(unit)) return;
            PartyUnits.Add(unit);
            unit.Alliance = UnitAlliance.Party;
        }

        public static void RemovePartyUnit(Unit unit)
        {
            if (!PartyUnits.Contains(unit)) return;
            PartyUnits.Remove(unit);
        }
    }
}