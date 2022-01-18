using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills.Basic
{
    public class Heal : Skill
    {
        private int fixedHeal;

        public Heal() : this(0) { }

        public Heal(int level) : base()
        {
            Name = "Heal";
            Priority = Common.AI.Priority.SmallerCurHP;
            Target = TargetType.Any;
            AITarget = TargetType.Friendly;
            Range = RangeType.Fixed;

            SpriteNumber = 563;
            InColor = Color.green;
            OutColor = Color.clear;

            species.Add(UnitSpecies.Human);
            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.LargeBeast);
            species.Add(UnitSpecies.Golem);

            OnUpgrade(level);
        }


        public override IEnumerator Use(Vector2Int target)
        {
            // 필요 변수 계산
            bool isCri = Random.Range(0, 100) < User.CriRate;

            int heal = fixedHeal;
            if (isCri) heal *= 2;

            // 스킬 소모 기록
            User.IsSkilled = true;
            WaitingTime = ReuseTime;
            Debug.Log($"{Name} 사용, {WaitingTime}을 {ReuseTime}으로 초기화");

            // 스킬 실행
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{User.Name}가 {Name}스킬을 {AITarget}타겟을 {Priority}우선으로 {targetUnit.Name}에 사용!");

                Common.Command.Heal(targetUnit, heal);
            }
            else
                Debug.Log($"{User.Name}가 {Name}스킬을 {AITarget}타겟을 {Priority}우선으로 {target}에 사용!");

            yield return null;
        }

        public override string GetDescription()
        {
            int heal = fixedHeal;

            return $"지정한 타일 위의 모든 대상에게 {heal}만큼 데미지를 준다.";
        }

        public override void OnUpgrade(int level)
        {
            Level = level;

            if (Level == 0)
            {
                ReuseTime = 2;
                APData = Common.Data.MakeRangeData(1, 3);
                RPData = Common.Data.MakeRangeData(1, 0);
                fixedHeal = 10;
            }
            else if (Level == 1)
            {
                ReuseTime = 2;
                APData = Common.Data.MakeRangeData(1, 4);
                RPData = Common.Data.MakeRangeData(1, 0);
                fixedHeal = 15;
            }
            else if (Level == 2)
            {
                ReuseTime = 1;
                APData = Common.Data.MakeRangeData(1, 5);
                RPData = Common.Data.MakeRangeData(1, 0);
                fixedHeal = 25;
            }
            else if (Level == 3)
            {
                ReuseTime = 1;
                APData = Common.Data.MakeRangeData(1, 6);
                RPData = Common.Data.MakeRangeData(1, 0);
                fixedHeal = 30;
            }
        }
    }
}