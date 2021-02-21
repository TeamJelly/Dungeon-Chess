using System.Collections.Generic;
using Common;
using Model.Units;
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
            AllUnits.Add(new Unit_001());
            AllUnits.Add(new Unit_000());

            AllUnits[0].UnitClass = UnitClass.Warrior;
            AllUnits[0].Move = 6;
            AllUnits[0].Agility = 10;
            UnitAction.AddSkill(AllUnits[0], new Model.Skills.Skill_000(), 0);
            UnitAction.AddSkill(AllUnits[0], new Model.Skills.Skill_005(), 1);

            AllUnits[1].UnitClass = UnitClass.Priest;
            AllUnits[1].Move = 7;
            AllUnits[1].Agility = 12;
            UnitAction.AddSkill(AllUnits[1], new Model.Skills.Skill_014(), 0);
            UnitAction.AddSkill(AllUnits[1], new Model.Skills.Skill_031(), 1);

        }
    }
}