using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
namespace Model.Managers
{
    public class UnitManager
    {
        //고용 가능한 모든 유닛들
        public List<Unit> AllUnits = new List<Unit>();

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
            AllUnits.Add(GetUnit("유닛1"));
            AllUnits.Add(GetUnit("유닛2"));
            AllUnits.Add(GetUnit("유닛3"));
        }

        /// <summary>
        /// 유닛 이름으로 유닛 반환. DB에서 불러오는게 좋을 듯 함.
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public static Unit GetUnit(string unitName)
        {
            Unit newUnit = new Unit(unitName);
            newUnit.agility = 3;
            newUnit.setSkill(new Skill_000(), 0);
            newUnit.setSkill(new Skill_001(), 1);
            newUnit.setSkill(new Skill_002(), 2);
            newUnit.setSkill(new Skills.DummySkill(), 3);
            return newUnit;
        }
        /// <summary>
        /// 배틀 씬에서 더미데이터 추가하는 코드.
        /// </summary>


    }
}