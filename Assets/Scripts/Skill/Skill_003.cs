using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_003 : Skill
{
    [Header("스킬 고유 특성")]
    public int damage;
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(owner.strength * damage + enhancedLevel);
        base.UseSkillToUnit(unit);
    }
}
