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

        UnitManager()
        {

        }

        /// <summary>
        /// 배틀 씬에서 더미데이터 추가하는 코드.
        /// </summary>
        public void InitForTesting()
        {
            PartyUnits.Add(new Unit("유닛1"));
            PartyUnits.Add(new Unit("유닛2"));
            PartyUnits.Add(new Unit("유닛3"));
            for(int i = 0; i < PartyUnits.Count; i++)
            {
                PartyUnits[i].agility = i + 1;
                PartyUnits[i].setSkill(new  Skill_000(),0);
                PartyUnits[i].setSkill(new  Skill_001(),1);
                PartyUnits[i].setSkill(new  Skill_002(),2);
                PartyUnits[i].setSkill(new  Model.Skills.DummySkill(),3);
            }
            EnemyUnits.Add(new Unit("몬스터2"));
            EnemyUnits.Add(new Unit("몬스터2"));
            for (int i = 0; i < EnemyUnits.Count; i++)
            {
                EnemyUnits[i].agility = i + 2;
                EnemyUnits[i].setSkill(new Skill_004(), 0);
                EnemyUnits[i].setSkill(new Skill_005(), 1);
                EnemyUnits[i].setSkill(new Skill_006(), 2);
                EnemyUnits[i].setSkill(new Skill_007(), 3);
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