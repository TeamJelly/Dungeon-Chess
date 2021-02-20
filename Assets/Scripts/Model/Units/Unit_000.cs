using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Unit_000 : Unit
    {
        public Unit_000() : base(0)
        {
            MoveSkill = new Walk();
            Skills[0] = new Skill_004();
            Skills[0].Level = 1;
            Skills[0] = new Skill_005();
            Skills[0].Level = 1;
            Skills[0] = new Skill_006();
            Skills[0].Level = 1;
            Skills[0] = new Skill_007();
            Skills[0].Level = 1;
        }
    }
}