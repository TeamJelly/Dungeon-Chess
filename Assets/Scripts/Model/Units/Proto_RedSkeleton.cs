using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Proto_RedSkeleton : Unit
    {
        public Proto_RedSkeleton()
        {
            MoveSkill.Priority = Common.AI.Priority.FarFromClosestParty;
            Skills[0].Target = Skill.TargetType.Party;
        }
    }
}