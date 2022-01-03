﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills.Basic
{
    public class Snapshot : BasicSkill
    {
        private float[] strToDam;
        private int[] fixedDam;
        public Snapshot() : base()
        {
            Name = "Snap Shot";
            Priority = Common.AI.Priority.NULL;
            Target = TargetType.Any;
            AITarget = TargetType.Hostile;
            Range = RangeType.Fixed;

            SpriteNumber = 324;
            InColor = Color.yellow;
            OutColor = Color.clear;

            ReuseTime = new int[4] { 1, 1, 0, 0 };
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
            strToDam = new float[4] { 1.0f, 1.5f, 2.0f, 2.5f };

            species.Add(UnitSpecies.SmallBeast);
            species.Add(UnitSpecies.MediumBeast);
            species.Add(UnitSpecies.Human);
        }

        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 필요 변수 계산
            int SLV = GetSLV(user);
            bool isCri = Random.Range(0, 100) < user.CriRate;
            int damage = (int)(user.Strength * strToDam[SLV]);
            if (isCri) damage *= 2;
            
            // 스킬 소모 기록
            user.IsSkilled = true;
            user.WaitingSkills.Add(this, ReuseTime[SLV]);

            // 스킬 실행
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {targetUnit.Name}에 사용!");

                Common.Command.Damage(targetUnit, damage);
            }
            else
                Debug.Log($"{user.Name}가 {Name}스킬을 {target}에 사용!");

            yield return null;
        }
        public override string GetDescription(Unit user)
        {
            int SLV = GetSLV(user);
            int damage = (int)(user.Strength * strToDam[SLV]);

            return $"지정한 대상에게 {damage}만큼 데미지를 준다.";
        }
    }
}