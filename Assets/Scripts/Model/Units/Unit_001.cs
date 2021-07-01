using Model.Skills;
using UnityEditor;
using UnityEngine;

namespace Model.Units
{
    public class Unit_001 : Unit
    {
        public Unit_001(): base(1)
        {
            MoveSkill = new Walk();
            MoveSkill.priority = Common.AI.Priority.NearFromClosestParty;
            Skills[0] = new Skill_000();
            Skills[0].target = Skill.TargetType.Party;
        }
    }
}