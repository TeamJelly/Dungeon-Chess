using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Proto_Skeleton : Unit
    {
        public Proto_Skeleton() : base(4)
        {
            MoveSkill.priority = Common.AI.Priority.NearFromClosestParty;
            Skills[0].target = Skill.TargetType.Party;
        }
    }
}