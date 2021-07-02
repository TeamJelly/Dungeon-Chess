using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Proto_Skeleton : Unit
    {
        public Proto_Skeleton()
        {
            MoveSkill.Priority = Common.AI.Priority.NearFromClosestParty;
            Skills[0].Target = Skill.TargetType.Party;
        }
    }
}