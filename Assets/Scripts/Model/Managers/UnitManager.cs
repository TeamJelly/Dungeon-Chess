using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
namespace Model.Managers
{
    public class UnitManager
    {
        public List<Unit> AllUnits = new List<Unit>();


        public List<Unit> PartyUnits = new List<Unit>();
        public List<Unit> EnemyUnits = new List<Unit>();

        static UnitManager instance;
        public static UnitManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new UnitManager();
                return instance;
            }
        }
        public void AddPartyUnit(Unit unit)
        {
            PartyUnits.Add(unit);
        }

        public void SubPartyUnit(Unit unit)
        {
            PartyUnits.Remove(unit);
        }

        public void AddEnemyUnit(Unit unit)
        {
            EnemyUnits.Add(unit);
        }
        public void SubEnemyUnit(Unit unit)
        {
            EnemyUnits.Remove(unit);
        }
    }
}