using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_009 : Skill
    {
        Extension_009 parsedExtension;
        public Extension_009 ParsedExtension => parsedExtension;
        public Skill_009() : base(9)
        {
            if(extension.Length > 0)
            {
                parsedExtension = Common.Extension.Parse<Extension_009>(extension);
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.SkillCount--;
            currentReuseTime = reuseTime;

            //Strength *2.0 + Strength* 0.2 * 강화 횟수
            float damage = user.Strength * ParsedExtension.strengthToDamageRatio + user.Strength * ParsedExtension.upgradePerEnhancedLevel;

            Unit targetUnit = Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {targetUnit.Name}에 사용!");

                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
                user.animationState = Unit.AnimationState.Attack;
                yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
                targetUnit.animationState = Unit.AnimationState.Hit;

                // 2단계 : 스킬 적용. 자신을 제외한 범위 안에 있는 대상에게 X의 데미지를 준다. 
                // 만약 그 데미지를 준 유닛이 죽었을 경우, 이 스킬을 다시 사용한다.
                Common.UnitAction.Damage(targetUnit, (int)Math.Round(damage));
            }
            else
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {target}에 사용!");
            }


        }
        public override string GetDescription(Unit user, int level)
        {
            float damage = user.Strength * ParsedExtension.strengthToDamageRatio + user.Strength * ParsedExtension.upgradePerEnhancedLevel;
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            //10% + 10% * 강화 횟수 
            criticalRate = 10 + 10 * level;

        }
    }

    [System.Serializable]
    public class Extension_009 : Common.Extensionable
    {
        public int strengthToDamageRatio;
        public float upgradePerEnhancedLevel;
    }
}