using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

/// <summary>
/// 스킬 이름: 베기
/// </summary>
public class Skill_000 : Skill
{
    private int strengthToDamageRatio;

    public Skill_000()
    {
        number = 0;
        name = "베기";
        unitClass = UnitClass.Warrior;
        grade = Grade.Normal;
        spritePath = "HandMade/SkillImage/000_베기";
        description = "한칸 안에 있는 단일 적에게 데미지를 입힌다.";
        criticalRate = 5;
        reuseTime = 0;
        APSchema = "3;010;101;010";
        RPSchema = "1;1";
        strengthToDamageRatio = 1;
    }

    public override IEnumerator Use(Unit user, Vector2Int target)
    {
        // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
        Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
        user.SkillCount--;
        currentReuseTime = reuseTime;

        int damage = user.Strength * strengthToDamageRatio + enhancedLevel * 2;

        if (targetUnit != null)
        {
            Debug.Log(name + " 스킬을 " + targetUnit.Name + "에 사용!");
            user.animationState = Unit.AnimationState.Attack;

            // 2단계 : Acttack 후에 맞는 애니메이션, HP갱신 재생
            yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
            targetUnit.animationState = Unit.AnimationState.Hit;
            Common.UnitAction.Damage(targetUnit, damage);
        }

    }
}
