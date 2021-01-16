﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 이름: 베기
/// </summary>
public class Skill_000 : Skill
{
    [Header("스킬 고유 특성")]
    public int damage;
    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(owner.strength * damage + enhancedLevel * 2);
        base.UseSkillToUnit(unit);
    }
}
