using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 이름: 강타(Colored_555)
/// </summary>
public class Skill_004 : Skill
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
