using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_001 : Skill
{

    public override void EnhanceSkill(int level)
    {
        enhancedLevel = level;
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError("hell");
        base.UseSkillToUnit(unit);
    }

}