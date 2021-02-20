using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_005 : Skill
    {
        Extension_005 parsedExtension;
        public Extension_005 ParsedExtension => parsedExtension;
        public Skill_005() : base(5)
        {
            if(extension.Length > 0)
            {
                parsedExtension = Common.Extension.Parse<Extension_005>(extension);
            }

        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.SkillCount--;
            currentReuseTime = reuseTime;

            //1.0 Strength + 강화 횟수 x 1
            int damage = user.Strength + level * parsedExtension.upgradePerEnhancedLevel;

            Unit targetUnit = Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {targetUnit.Name}에 사용!");

                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
                user.animationState = Unit.AnimationState.Attack;
                yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
                targetUnit.animationState = Unit.AnimationState.Hit;

                // 2단계 : 스킬 적용
                Common.UnitAction.Damage(targetUnit, damage);
            }
            else
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {target}에 사용!");
            }
        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = user.Strength + level * parsedExtension.upgradePerEnhancedLevel;
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }
        public override void Upgrade()
        {
            if (level < 10)
                base.Upgrade();
        }
    }
    [System.Serializable]
    public class Extension_005 : Common.Extensionable
    {
        public int upgradePerEnhancedLevel;
    }
}