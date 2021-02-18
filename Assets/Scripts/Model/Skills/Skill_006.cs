using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_006 : Skill
    {
        Extension_006 parsedExtension;
        public Extension_006 ParsedExtension => parsedExtension;
        public Skill_006() : base(6)
        {
            if (extension != null)
            {
                parsedExtension = Common.Extension.Parse<Extension_006>(extension);
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            user.SkillCount--;
            currentReuseTime = reuseTime;

            int defense = 15 + enhancedLevel * parsedExtension.upgradePerEnhancedLevel;

            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {targetUnit.Name}에 사용!");
                user.animationState = Unit.AnimationState.Attack;

                // 2단계 : Acttack 후에 맞는 애니메이션, HP갱신 재생
                yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
                targetUnit.animationState = Unit.AnimationState.Hit;
                
                //방어도 높이는 상태효과 발동.
            }
            else
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {target}에 사용!");
            }

        }
        public override string GetDescription(Unit user, int level)
        {
            int defense = 15 + level * parsedExtension.upgradePerEnhancedLevel;
            string str = base.GetDescription(user, level).Replace("X", defense.ToString());
            return str;
        }
    }

        [System.Serializable]
    public class Extension_006 : Common.Extensionable
    {
        public int upgradePerEnhancedLevel;
    }
}