using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Proto_RedSkeleton : Unit
    {
        public Proto_RedSkeleton() : base(5)
        {
            Skills[0].target = Skill.Target.Party;
        }
    }
}