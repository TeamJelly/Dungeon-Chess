using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_001 : Skill
{
    Skill_001()
    {
        number = 1;
        name = "베기";
        unitClass = UnitClass.Warrior;

        grade = Grade.NULL;

        description = "한칸 안에 있는 단일 적에게 데미지를 입힌다.";
        enhancedLevel = 0;
        reuseTime = 0;

        criticalRate = 5;
    }
}
