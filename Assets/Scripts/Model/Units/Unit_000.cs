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
            Skills[1] = new Skill_005();
            Skills[1].Level = 1;
            Skills[2] = new Skill_006();
            Skills[2].Level = 1;
            Skills[3] = new Skill_007();
            Skills[3].Level = 1;
        }
    }
}