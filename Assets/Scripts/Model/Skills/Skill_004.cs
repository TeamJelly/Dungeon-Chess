using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_004 : Skill
    {
        Extension_004 parsedExtension;
        public Extension_004 ParsedExtension => parsedExtension;
        public Skill_004() : base(4)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_004>(extension);
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            user.SkillCount--;
            currentReuseTime = reuseTime;

            int damage = user.Strength * 10 + enhancedLevel * parsedExtension.upgradePerEnhancedLevel;

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
            Common.UnitAction.AddEffect(targetUnit, new Effects.Effect_004(targetUnit));

        }
        public override string GetDescription(Unit user)
        {
            int damage = user.Strength * 10 + enhancedLevel * parsedExtension.upgradePerEnhancedLevel;
            return $"{base.GetDescription(user)}\n Damage: {damage}";
        }
    }

        [System.Serializable]
    public class Extension_004 : Extensionable
    {
       public int upgradePerEnhancedLevel;
    }
}