using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class S002_MagicArrow : Skill
    {
        public S002_MagicArrow()
        {
            Name = "마법화살";
            Number = 2;
            MaxLevel = 3;
            ReuseTime = 0;
            CriticalRate = 0;

            Priority = Common.AI.Priority.NULL;
            Target = TargetType.Any;
            Range = RangeType.Fixed;

            Sprite = Common.Data.LoadSprite("HandMade/SkillImage/002_마법화살");
            Description =
                $"선택한 대상에게 X의 데미지를 준다." +
                $"\n\n\n‘굳이 화살로 만들어서 쏴야하나..?’";

            APData = "7;0001000;0011100;0111110;1110111;0111110;0011100;0001000";
            RPData = "1;1";
        }

        private readonly int strToDmg = 1;
        private readonly int grdToDmg = 1;

        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.IsSkilled = true;
            user.WaitingSkills[this] = ReuseTime;

            //Strength + 강화 횟수 x 1
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
        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = user.Strength * strToDmg + Level + grdToDmg;
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }
    }
}