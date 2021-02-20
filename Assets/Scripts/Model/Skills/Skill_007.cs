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
            user.SkillCount--;
            currentReuseTime = reuseTime;

            //5+ 강화 횟수 
            int defense = 5 + level;

            Unit targetUnit = Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {targetUnit.Name}에 사용!");

                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
                user.animationState = Unit.AnimationState.Attack;
                yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
                targetUnit.animationState = Unit.AnimationState.Heal;

                // 2단계 : 스킬 적용. 지정 대상과 자신에게 x + 1의 방어도와 1의 보호막 부여.
                Common.UnitAction.Armor(user, 1);
                Common.UnitAction.Armor(targetUnit, 1);
                Common.UnitAction.AddEffect(user, new Effect_021(user, 1,1));
                Common.UnitAction.AddEffect(targetUnit, new Effect_021(targetUnit, 1,1));
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
        public override void Upgrade()
        {
            if (level < 10)
                base.Upgrade();
        }
    }

    [System.Serializable]
    public class Extension_007 : Common.Extensionable
    {
        public int upgradePerEnhancedLevel;
    }
}