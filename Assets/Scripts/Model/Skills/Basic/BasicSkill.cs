using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills.Basic
{
    public class BasicSkill : Skill
    {
        public BasicSkill()
        {
            Category = SkillCategory.Basic;
        }

        public override int GetSLV (Unit user)
        {
            if (user.Level == 1)
                return 1;
            else if (2 <= user.Level && user.Level < 4)
                return 2;
            else if (4 <= user.Level && user.Level < 7)
                return 3;
            else if (7 <= user.Level)
                return 4;
            else
                return -1;
        }
    }
}