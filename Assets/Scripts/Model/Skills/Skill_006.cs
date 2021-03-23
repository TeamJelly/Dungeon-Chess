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
            if(extension.Length > 0)
            {
                parsedExtension = Common.Extension.Parse<Extension_006>(extension);
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.SkillCount--;
            CurrentReuseTime = reuseTime;
            //15 + 강화 횟수 x 2
            int defense = 15 + Level * parsedExtension.upgradePerEnhancedLevel;

            Unit targetUnit = Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {targetUnit.Name}에 사용!");
                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
                yield return null;

                // 2단계 : 스킬 적용. 방어도 높이는 상태효과 발동.
                Common.UnitAction.Armor(user, defense);
            }
            else
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {target}에 사용!");
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