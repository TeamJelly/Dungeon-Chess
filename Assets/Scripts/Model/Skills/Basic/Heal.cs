using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills.Basic
{
    public class Heal : Skill
    {
        private int fixedHeal;

        public Heal() : base()
        {
            Name = "Heal";
            Priority = Common.AI.Priority.SmallerCurHP;
            Target = TargetType.Any;
            AITarget = TargetType.Friendly;
            Range = RangeType.Fixed;

            SpriteNumber = 563;
            InColor = Color.green;
            OutColor = Color.clear;

            ReuseTime = new int[4] { 2, 2, 1, 1 };
            APData = new string[4]
            {
                Common.Data.MakeRangeData(1, 3),
                Common.Data.MakeRangeData(1, 3),
                Common.Data.MakeRangeData(1, 3),
                Common.Data.MakeRangeData(1, 3),
            };
            RPData = new string[4]
            {
                Common.Data.MakeRangeData(1, 0),
                Common.Data.MakeRangeData(1, 0),
                Common.Data.MakeRangeData(1, 0),
                Common.Data.MakeRangeData(1, 0),
            };
            fixedHeal = new int[4] { 10, 15, 25, 30 };

            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.LargeBeast);
            species.Add(UnitSpecies.Golem);
        }


        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 필요 변수 계산
            int SLV = GetSLV(user);
            bool isCri = Random.Range(0, 100) < user.CriRate;
            int heal = fixedHeal[SLV];
            if (isCri) heal *= 2;

            // 스킬 소모 기록
            user.IsSkilled = true;
            user.WaitingSkills.Add(this, ReuseTime[SLV]);

            // 스킬 실행
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {targetUnit.Name}에 사용!");

                Common.Command.Heal(targetUnit, heal);
            }
            else
                Debug.Log($"{user.Name}가 {Name}스킬을 {target}에 사용!");

            yield return null;
        }

        public override string GetDescription(Unit user)
        {
            int SLV = GetSLV(user);
            int heal = fixedHeal[SLV];

            return $"지정한 타일 위의 모든 대상에게 {heal}만큼 데미지를 준다.";
        }
    }
}