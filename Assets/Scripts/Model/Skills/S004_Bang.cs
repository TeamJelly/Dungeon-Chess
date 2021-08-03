using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class S004_Bang : Skill
    {
        public S004_Bang()
        {
            Name = "강타";
            Number = 4;
            MaxLevel = 3;
            ReuseTime = 3;
            CriticalRate = 0;

            Priority = Common.AI.Priority.NULL;
            Target = TargetType.Any;
            Range = RangeType.Fixed;

            Sprite = Common.Data.LoadSprite("HandMade/SkillImage/004_강타");
            Description = 
                $"선택한 대상에게 X의 데미지를 입히고, 한턴간 기절시킨다.\n\n\n" +
                $"‘그냥 힘 줘서 때리면 기절 하더라고.’";

            APData = "3;010;101;010";
            RPData = "1;1";
            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.Golem);

            Category = SkillCategory.Intermediate;
        }

        private readonly int grdToDmg = 1;

        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산  
            user.SkillCount--;
            CurReuseTime = ReuseTime;

            //10 + 강화 횟수 x 1
            int damage = 10 + Level * grdToDmg;
            Unit targetUnit = Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {targetUnit.Name}에 사용!");

                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
                yield return null;

                // 2단계 : 스킬 적용
                Common.Command.Damage(targetUnit, damage);
            }
            else
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {target}에 사용!");
            }
            Common.Command.AddEffect(targetUnit, new Effects.E004_Stun(targetUnit));

        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = 10 + Level * grdToDmg;
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }
    }
}