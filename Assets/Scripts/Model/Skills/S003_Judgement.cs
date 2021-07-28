using System.Collections;
using UnityEngine;

namespace Model.Skills
{
    public class S003_Judgement : Skill
    {
        public S003_Judgement()
        {
            Name = "천벌";
            Number = 3;
            MaxGrade = 3;
            ReuseTime = 0;
            CriticalRate = 0;

            Priority = Common.AI.Priority.NULL;
            Target = TargetType.Any;
            Range = RangeType.Fixed;

            spritePath = "HandMade/SkillImage/003_천벌";
            Description =
                $"두칸 안에 있는 단일 대상에게 [STR*{strToDmg}+GRD*{grdToDmg}]의 데미지를 준다.\n\n\n" +
                $"‘벼락이 아니라, 신의 심판입니다.’";
            APData =
                "5;" +
                "00100;" +
                "01110;" +
                "11011;" +
                "01110;" +
                "00100";
            RPData = "1;1";
        }

        private readonly int strToDmg = 1;
        private readonly int grdToDmg = 1;

        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.SkillCount--;
            CurReuseTime = ReuseTime;

            //Strength + 강화 횟수 x 1
            int damage = user.Strength * strToDmg + Grade * grdToDmg;

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
            int damage = user.Strength * strToDmg + level * grdToDmg;
            string str = base.GetDescription(user, level).Replace($"[STR*{strToDmg}+GRD*{grdToDmg}]", damage.ToString());
            return str;
        }
    }
}