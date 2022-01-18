using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills.Basic
{
    public class Slash : Skill
    {
        private float strToDam;
        private int fixedDam;

        public Slash() : this(0) { }

        public Slash(int level) : base()
        {
            Name = "Slash";
            Priority = Common.AI.Priority.NULL;
            UserTarget = TargetType.Any;
            AITarget = TargetType.Hostile;
            Range = RangeType.Fixed;

            SpriteNumber = 551;
            InColor = Color.red;
            OutColor = Color.clear;

            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.Human);

            OnUpgrade(level);
        }

        public override IEnumerator Use(Vector2Int target)
        {
            // 필요 변수 계산
            bool isCri = Random.Range(0, 100) < User.CriRate;
            int damage = (int)(fixedDam + User.Strength * strToDam);
            if (isCri) damage *= 2;
            
            // 스킬 소모 기록
            User.IsSkilled = true;
            WaitingTime = ReuseTime;

            // 스킬 실행
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{User.Name}가 {Name}스킬을 {AITarget}타겟을 {Priority}우선으로 {targetUnit.Name}에 사용!");
                Common.Command.Damage(targetUnit, damage);
            }
            else
                Debug.Log($"{User.Name}가 {Name}스킬을 {AITarget}타겟을 {Priority}우선으로 {target}에 사용!");

            yield return null;
        }

        public override string GetDescription()
        {
            int damage = (int)(fixedDam + User.Strength * strToDam);

            return $"지정한 대상에게 {damage}만큼 데미지를 준다.";
        }

        public override void OnUpgrade(int level)
        {
            Level = level;

            if (Level == 0)
            {
                ReuseTime = 0;
                APData = Common.Data.MakeRangeData(1, 1);
                RPData = Common.Data.MakeRangeData(1, 0);
                strToDam = 1.5f;
                fixedDam = 0;
            }
            else if (Level == 1)
            {
                ReuseTime = 0;
                APData = Common.Data.MakeRangeData(1, 1);
                RPData = Common.Data.MakeRangeData(1, 0);
                strToDam = 2f;
                fixedDam = 0;
            }
            else if (Level == 2)
            {
                ReuseTime = 0;
                APData = Common.Data.MakeRangeData(1, 1);
                RPData = Common.Data.MakeRangeData(1, 0);
                strToDam = 2.5f;
                fixedDam = 0;
            }
            else if (Level == 3)
            {
                ReuseTime = 0;
                APData = Common.Data.MakeRangeData(1, 1);
                RPData = Common.Data.MakeRangeData(1, 0);
                strToDam = 3f;
                fixedDam = 0;
            }
        }
    }
}