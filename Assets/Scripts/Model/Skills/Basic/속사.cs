using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills.Basic
{
    public class 속사 : Skill
    {
        public 속사()
        {
            Name = "속사";
            //Number = 1;
            MaxLevel = 4;
            ReuseTime = 1;
            CriticalRate = 10;

            Priority = Common.AI.Priority.NULL;
            Target = TargetType.Any;
            Range = RangeType.Fixed;

            Sprite = Common.Data.LoadSprite("HandMade/SkillImage/001_속사");
            Description =
                $"지정한 타일 위의 모든 대상에게 데미지를 준다.\n\n\n" +
                $"‘속사 관련 대사.’";

            APData = "3;010;101;010";
            RPData = "1;1";

            Category = SkillCategory.Basic;

            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.Human);
        }

        private float strToDmg = 1;

        public override void Upgrade()
        {
            base.Upgrade();

            switch (Level)
            {
                case 1:
                    strToDmg = 1;
                    ReuseTime = 1;
                    break;
                case 2:
                    strToDmg = 1.5f;
                    ReuseTime = 1;
                    break;
                case 3:
                    strToDmg = 1.5f;
                    ReuseTime = 0;
                    break;
                case 4:
                    strToDmg = 2;
                    ReuseTime = 0;
                    break;
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.SkillCount--;
            CurReuseTime = ReuseTime;

            int damage = (int)(user.Strength * strToDmg);

            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
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
        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = (int)(user.Strength * strToDmg);
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }
    }
}