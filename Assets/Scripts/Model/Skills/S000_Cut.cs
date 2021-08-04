using System.Collections;
using UnityEngine;

namespace Model.Skills
{
    public class S000_Cut : Skill
    {
        public S000_Cut()
        {
            Name = "베기";
            Number = 0;
            MaxLevel = 3;
            ReuseTime = 0;
            CriticalRate = 5;

            Priority = Common.AI.Priority.NULL;
            Target = TargetType.Any;
            Range = RangeType.Fixed;

            Sprite = Common.Data.LoadSprite("HandMade/SkillImage/000_베기");
            Description =
                $"선택한 대상에게 X의 데미지를 준다.\n\n\n" +
                $"‘반으로 갈라져 죽어버려.’";

            APData = "3;010;101;010";
            RPData = "1;1";

            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.LargeBeast);
            species.Add(UnitSpecies.Golem);

            Category = SkillCategory.Basic;
        }

        private readonly int strToDmg = 1;
        private readonly int grdToDmg = 2;

        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.SkillCount--;
            CurReuseTime = ReuseTime;

            //Strength + 강화 횟수 x 2
            int damage = user.Strength * strToDmg + Level + grdToDmg;

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

            View.VisualEffectView.MakeVisualEffect(target, "Scratch");
        }

        public override string GetDescription(Unit user, int grade)
        {
            int damage = user.Strength * strToDmg + grade * grdToDmg;
            string str = base.GetDescription(user, grade).Replace("X", damage.ToString());
            return str;
        }
    }
}