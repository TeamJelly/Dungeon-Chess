using Model.Skills;
using UnityEditor;
using UnityEngine;

namespace Model.Units
{
    public class Unit_001 : Unit
    {
        public Unit_001()
        {
            MoveSkill = new S100_Walk();
            MoveSkill.Priority = Common.AI.Priority.NearFromClosestParty;
            Skills[0] = new S000_Cut();
            Skills[0].Target = Skill.TargetType.Party;
        }
    }
}