using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Proto_Judgement : Unit
    {
        public Proto_Judgement() : base(6)
        {
            MoveSkill.priority = Common.AI.Priority.NearFromClosestParty;
            Skills[0].target = Skill.TargetType.Party;
            Skills[0].Level = 5;
            Skills[1].target = Skill.TargetType.Party;
            Skills[1].Level = 3;
            Skills[2].target = Skill.TargetType.Party;
            Skills[2].Level = 2;
        }
    }
}
