using Model.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_007 : Skill
    {
        Extension_007 parsedExtension;
        public Extension_007 ParsedExtension => parsedExtension;
        public Skill_007() : base(7)
        {
            if (extension != null)
            {
                parsedExtension = Common.Extension.Parse<Extension_007>(extension);
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            user.SkillCount--;
            currentReuseTime = reuseTime;

            int defense = 5 + enhancedLevel;

            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {targetUnit.Name}에 사용!");
                user.animationState = Unit.AnimationState.Attack;

                // 2단계 : Acttack 후에 맞는 애니메이션, HP갱신 재생
                yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
                targetUnit.animationState = Unit.AnimationState.Hit;

                //지정 대상과 자신에게 x + 1의 방어도와 1의 보호막 부여.
                Common.UnitAction.Armor(user, 1);
                Common.UnitAction.Armor(targetUnit, 1);
                Common.UnitAction.AddEffect(user, new Effect_021(user, 1));
                Common.UnitAction.AddEffect(targetUnit, new Effect_021(targetUnit, 1));
            }
            else
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {target}에 사용!");
            }
            

        }
        public override string GetDescription(Unit user, int level)
        {
            int defense = 5 + level;
            string str = base.GetDescription(user, level).Replace("X", defense.ToString());
            return str;
        }
    }

    [System.Serializable]
    public class Extension_007 : Common.Extensionable
    {
        public int upgradePerEnhancedLevel;
    }
}