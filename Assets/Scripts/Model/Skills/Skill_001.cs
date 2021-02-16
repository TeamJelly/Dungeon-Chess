using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_001 : Skill
    {
        Extension_001 parsedExtension;
        public Extension_001 ParsedExtension => parsedExtension;
        public Skill_001() : base(1)
        {
            if (extension != null)
            {
                parsedExtension = Common.Extension.Parse<Extension_001>(extension);
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
        public override string GetDescription(Unit user)
        {
            int damage = user.Strength * parsedExtension.strengthToDamageRatio + enhancedLevel * parsedExtension.upgradePerEnhancedLevel;
            return $"{base.GetDescription(user)}\n Damage: {damage}";
        }
    }

        [System.Serializable]
    public class Extension_001 : Common.Extensionable
    {
        public int strengthToDamageRatio;
        public int upgradePerEnhancedLevel;
    }
}