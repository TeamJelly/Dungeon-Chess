﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    /// <summary>
    /// 스킬 이름: 마법 화살(Colored_280)
    /// </summary>
    public class Skill_002 : Skill
    {
        Extension_002 parsedExtension;
        public Extension_002 ParsedExtension => parsedExtension;
        public Skill_002() : base(2)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_002>(extension);
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            user.SkillCount--;
            currentReuseTime = reuseTime;

            int damage = user.Strength * parsedExtension.strengthToDamageRatio + enhancedLevel * parsedExtension.upgradePerEnhancedLevel;

            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {targetUnit.Name}에 사용!");
                user.animationState = Unit.AnimationState.Attack;

                // 2단계 : Acttack 후에 맞는 애니메이션, HP갱신 재생
                yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
                targetUnit.animationState = Unit.AnimationState.Hit;
                Common.UnitAction.Damage(targetUnit, damage);
            }
            else
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {target}에 사용!");
            }
        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = user.Strength * parsedExtension.strengthToDamageRatio + level * parsedExtension.upgradePerEnhancedLevel;
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }
    }

    [System.Serializable]
    public class Extension_002 : Extensionable
    {
        public int strengthToDamageRatio;
        public int upgradePerEnhancedLevel;
    }
}