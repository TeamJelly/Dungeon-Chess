using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class S005_SpinSlash : Skill
    {
        public S005_SpinSlash()
        {
            Name = "회전베기";
            Number = 5;
            MaxGrade = 3;
            ReuseTime = 1;
            CriticalRate = 5;

            Priority = Common.AI.Priority.NULL;
            Target = TargetType.Any;
            Range = RangeType.Fixed;

            Sprite = Common.Data.LoadSprite("HandMade/SkillImage/005_회전베기");
            Description =
                $"범위안의 모든 대상에게 X의 데미지를 입힌다.\n\n\n" +
                $"‘ 회전~ 회오리~’";

            APData = "1;1";
            RPData = "3;111;101;111";
        }

        private readonly int strToDmg = 1;
        private readonly int grdToDmg = 2;

        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.SkillCount--;
            CurReuseTime = ReuseTime;

            //1.0 Strength + 강화 횟수 x 1
            int damage = user.Strength * strToDmg + Grade + grdToDmg;

            List<Unit> targetUnits = new List<Unit>();
            foreach (Vector2Int vector in GetRelatePositions(user, user.Position))
            {
                Unit targetUnit = FieldManager.GetTile(vector).GetUnit();
                if (targetUnit != null)
                {
                    targetUnits.Add(targetUnit);
                }
            }

            foreach (Unit unit in targetUnits)
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {unit.Name}에 사용!");
                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
                yield return null;
                // 2단계 : 스킬 적용
                Common.Command.Damage(unit, damage);
            }
        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = user.Strength * strToDmg + Grade + grdToDmg;
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }
    }
}