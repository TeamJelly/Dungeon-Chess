using System.Collections;
using UnityEngine;

namespace Model.Skills
{
    /// <summary>
    /// 스킬 이름: 베기
    /// </summary>
    public class Skill_000  : Skill
    {
        Extension_000 parsedExtension;
        public Extension_000 ParsedExtension => parsedExtension;
        public Skill_000() : base(0) 
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_000>(extension);
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target) {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            user.SkillCount--;
            currentReuseTime = reuseTime;

            int damage = user.Strength * parsedExtension.strengthToDamageRatio + enhancedLevel * parsedExtension.upgradePerEnhancedLevel;

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

    public class Extension_000 : Extensionable
    {
        public int strengthToDamageRatio;
        public int upgradePerEnhancedLevel;
    }
}