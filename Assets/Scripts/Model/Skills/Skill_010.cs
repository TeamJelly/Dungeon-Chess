using Model.Effects;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_010 : Skill
    {
        Extension_010 parsedExtension;
        public Extension_010 ParsedExtension => parsedExtension;
        public Skill_010() : base(10)
        {
            if(extension.Length > 0)
            {
                parsedExtension = Common.Extension.Parse<Extension_010>(extension);
            }
        }
        private int tauntTurnCount = 2;
        private int protectionTurnCount = 2;
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.SkillCount--;
            CurrentReuseTime = reuseTime;

            //40 + if (enhance ==  4) + 20
            int value = 40 + Level == 4 ? 20 : 0;

            Unit targetUnit = Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {targetUnit.Name}에 사용!");

                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
                yield return null;

                // 2단계 : 스킬 적용
                foreach (Unit unit in BattleManager.GetUnit(Alliance.Enemy))
                    Common.UnitAction.AddEffect(unit, new Effect_013(unit, tauntTurnCount));
                Common.UnitAction.AddEffect(user, new Effect_021(user, 1,protectionTurnCount));
            }
            else
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {target}에 사용!");
            }
        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = 40 + level == 4 ? 20 : 0;
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }

        /*
        + 1 : 모든 적에게 도발 효과를 2턴간 부여하고, 자신에게 3턴간 X 만큼의 보호막을 건다.
        + 2 : 모든 적에게 도발 효과를 3턴간 부여하고, 자신에게 3턴간 X 만큼의 보호막을 건다.
        + 3 : 재사용 대기시간 6->5턴
        + 4 : 모든 적에게 도발 효과를 3턴간 부여하고, 자신에게 3턴간 X+20 만큼의 보호막을 건다.
        + 5 : 재사용 대기시간 5->4턴
        */
        public override void Upgrade()
        {
            base.Upgrade();
            switch (Level)
            {
                case 1:
                    tauntTurnCount = 2;
                    protectionTurnCount = 3;
                    break;
                case 2:
                    tauntTurnCount = 3;
                    protectionTurnCount = 3;
                    break;
                case 3:
                    reuseTime = 5;
                    break;
                case 4:
                    tauntTurnCount = 3;
                    protectionTurnCount = 3;
                    break;
                case 5:
                    reuseTime = 4;
                    break;
            }
        }
    }

        [System.Serializable]
    public class Extension_010 : Common.Extensionable
    {
        public float upgradePerEnhancedLevel;
    }
}