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

        public int stage = 1;
        public int Gold { get => gold; set => gold = value; }
        int gold;

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
            unit.Category = Category.Party;
        }

        public static void RemovePartyUnit(Unit unit)
        {
            PartyUnits.Remove(unit);
        }

        public void InitForTesting()
        {
            partyUnits.Add(new Units.Unit_000
            {
                Name = "파티원 1",
                Category = Category.Party,
                Position = new Vector2Int(4, 1),
                Move = 6
            });
            Common.UnitAction.AddEffect(partyUnits[0], new Effects.Effect_004(partyUnits[0]));
            Common.UnitAction.AddEffect(partyUnits[0], new Effects.Effect_005(partyUnits[0]));

            partyUnits.Add(new Units.Unit_000
            {
                Name = "party2",
                Category = Category.Party,
                Position = new Vector2Int(5, 1),
                Agility = 11,
                Move = 7
            });
            //partyUnits.Add(new Units.Unit_000
            //{
            //    Name = "party3",
            //    Category = Category.Party,
            //    Position = new Vector2Int(6, 1),
            //    Move = 8
            //});
            //partyUnits.Add(new Units.Unit_000
            //{
            //    Name = "party3",
            //    Category = Category.Party,
            //    Position = new Vector2Int(7, 1),
            //    Move = 8
            //});
            //partyUnits.Add(new Units.Unit_000
            //{
            //    Name = "party3",
            //    Category = Category.Party,
            //    Position = new Vector2Int(8, 1),
            //    Move = 8
            //});
            //partyUnits.Add(new Units.Unit_000
            //{
            //    Name = "party3",
            //    Category = Category.Party,
            //    Position = new Vector2Int(6, 2),
            //    Move = 8
            //});
            //partyUnits.Add(new Units.Unit_000
            //{
            //    Name = "party3",
            //    Category = Category.Party,
            //    Position = new Vector2Int(6, 3),
            //    Move = 8
            //});
            //partyUnits.Add(new Units.Unit_000
            //{
            //    Name = "party3",
            //    Category = Category.Party,
            //    Position = new Vector2Int(6, 4),
            //    Move = 8
            //});
            //partyUnits.Add(new Units.Unit_000
            //{
            //    Name = "party3",
            //    Category = Category.Party,
            //    Position = new Vector2Int(6, 5),
            //    Move = 8
            //});
        }
    }
}