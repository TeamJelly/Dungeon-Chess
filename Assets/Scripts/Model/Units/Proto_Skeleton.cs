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
            Skills[0].target = Skill.Target.Party;
            Skills[1].target = Skill.Target.Party;
        }
    }
}