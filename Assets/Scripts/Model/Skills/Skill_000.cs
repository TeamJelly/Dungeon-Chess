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
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            base.Use(user, target);

            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            int damage = user.Strength * parsedExtension.strengthToDamageRatio + enhancedLevel * parsedExtension.upgradePerEnhancedLevel;

            // 1단계 : Idle이면 Attack 애니메이션 재생
            yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
            user.animationState = Unit.AnimationState.Attack;


            // 2단계 : Acttack 후에 맞는 애니메이션, HP갱신 재생
            yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
            targetUnit.animationState = Unit.AnimationState.Hit;
            Common.UnitAction.Damage(targetUnit, damage);
            //Effect.StartEffect("베기", target);

            yield return new WaitWhile(() => targetUnit.animationState != Unit.AnimationState.Idle);
        }
    }

    [System.Serializable]
    public class Extension_000 : Extensionable
    {
        public int strengthToDamageRatio;
        public int upgradePerEnhancedLevel;
    }
}