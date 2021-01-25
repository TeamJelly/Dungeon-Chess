using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 이름: 
/// </summary>
public class Skill_008 : Skill
{
    [Header("스킬 고유 특성")]
    private int strengthToDamageRatio;
    Skill_008()
    {
        number = 8;
        name = "힘의 파동";
        unitClass = UnitClass.Warrior;
        grade = Grade.Rare;
        skillImagePath = "HandMade/SkillImage/008_힘의 파동";
        description = "범위 안에 있는 모든 적군에게 데미지를 입히고 기절 시킨다.";
        criticalRate = 0;
        reuseTime = 5;
        domain = Domain.Rotate;
        target = Target.AnyTile;
        APSchema = "3;010;111;010";
        // RPSchema = "3;111;111;000";//회전 들어감. 추후 수정.
        strengthToDamageRatio = 2;
    }

    string[] RPSchemas = { 
        "3;" +
            "111;" +
            "111;" +
            "000", // 0강
        
        "5;" +
            "01110;" +
            "01110;" +
            "01110;" +
            "00000;" +
            "00000", // 1강

        "7;" +
            "0011100;" +
            "0011100;" +
            "0011100;" +
            "0011100;" +
            "0000000;" +
            "0000000;" +
            "0000000", // 2강

        "9;" +
            "000111000;" +
            "000111000;" +
            "000111000;" +
            "000111000;" +
            "000111000;" +
            "000000000;" +
            "000000000;" +
            "000000000;" +
            "000000000", // 3강

        "11;" +
            "00001110000;" +
            "00001110000;" +
            "00001110000;" +
            "00001110000;" +
            "00001110000;" +
            "00001110000;" +
            "00000000000;" +
            "00000000000;" +
            "00000000000;" +
            "00000000000;" +
            "00000000000", // 4강

        "13;" +
            "0000011100000;" +
            "0000011100000;" +
            "0000011100000;" +
            "0000011100000;" +
            "0000011100000;" +
            "0000011100000;" +
            "0000011100000;" +
            "0000000000000;" +
            "0000000000000;" +
            "0000000000000;" +
            "0000000000000;" +
            "0000000000000;" +
            "0000000000000" // 5강
    };

    public override List<Vector2Int> GetRangePositions()
    {
        if (enhancedLevel <= 5)
            return Common.Range.ParseRangeSchema(RPSchemas[enhancedLevel]);
        else
            return Common.Range.ParseRangeSchema(RPSchemas[4]);
    }

    public override void UseSkillToUnit(Unit unit)
    {
        Debug.LogError(name + " 스킬을 " + unit.name + "에 사용!");
        unit.GetDamage(GetComponent<Unit>().strength * strengthToDamageRatio + enhancedLevel);
        base.UseSkillToUnit(unit);
    }
}
