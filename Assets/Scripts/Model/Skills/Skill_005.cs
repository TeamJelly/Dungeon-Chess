using Model.Managers;
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
            CurrentReuseTime = reuseTime;

            //1.0 Strength + 강화 횟수 x 1
            int damage = user.Strength + Level * parsedExtension.upgradePerEnhancedLevel;

            List<Unit> targetUnits = new List<Unit>();
            foreach(Vector2Int vector in GetRelatePositions(user, user.Position))
            {
                Unit targetUnit = BattleManager.GetTile(vector).GetUnit();
                if (targetUnit != null)
                {
                    targetUnits.Add(targetUnit);
                }
            }
            user.animationState = Unit.AnimationState.Attack;
            yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);

            foreach (Unit unit in targetUnits)
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {unit.Name}에 사용!");
                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
                unit.animationState = Unit.AnimationState.Hit;
                // 2단계 : 스킬 적용
                Common.UnitAction.Damage(unit, damage);
            }
        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = user.Strength + level * parsedExtension.upgradePerEnhancedLevel;
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }
    }
    [System.Serializable]
    public class Extension_005 : Common.Extensionable
    {
        public int upgradePerEnhancedLevel;
    }
}