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
            Skills[0].target = Skill.Target.Party;
            Skills[1].target = Skill.Target.Party;
        }
    }
}
