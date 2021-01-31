using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Model.Managers
{
    public class GameManager
    {
        static GameManager instance;

        public List<Vector2Int> roomHistory = new List<Vector2Int>();
        public Room currentRoom = null;
        public Room[,] AllRooms = null;
        public List<Vector2Int>[] pathList = null;
        List<Unit> partyUnits = new List<Unit>();

        public static List<Unit> PartyUnits { get => Instance.partyUnits; }
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
        }

        public static void AddPartyUnit(Unit unit)
        {
            PartyUnits.Add(unit);
        }

        public static void SubPartyUnit(Unit unit)
        {
            PartyUnits.Remove(unit);
        }

        public void InitForTesting()
        {
            partyUnits.Add(UnitManager.GetUnit("유닛1"));
            partyUnits.Add(UnitManager.GetUnit("유닛2"));
            partyUnits.Add(UnitManager.GetUnit("유닛3"));
        }
    }
}