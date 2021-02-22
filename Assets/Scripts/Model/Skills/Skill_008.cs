using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_008 : Skill
    {
        Extension_008 parsedExtension;
        public Extension_008 ParsedExtension => parsedExtension;

        string[] RPSchemas = {
                "3;" +
                    "111;" +
                    "111;" +
                    "000", // 0강

                "5;" +
                    "01110;" +
                    "01110;" +
                    "01110;" +
                    "00000;" +
                    "00000", // 1강

                "7;" +
                    "0011100;" +
                    "0011100;" +
                    "0011100;" +
                    "0011100;" +
                    "0000000;" +
                    "0000000;" +
                    "0000000", // 2강

                "9;" +
                    "000111000;" +
                    "000111000;" +
                    "000111000;" +
                    "000111000;" +
                    "000111000;" +
                    "000000000;" +
                    "000000000;" +
                    "000000000;" +
                    "000000000", // 3강

                "11;" +
                    "00001110000;" +
                    "00001110000;" +
                    "00001110000;" +
                    "00001110000;" +
                    "00001110000;" +
                    "00001110000;" +
                    "00000000000;" +
                    "00000000000;" +
                    "00000000000;" +
                    "00000000000;" +
                    "00000000000", // 4강

                "13;" +
                    "0000011100000;" +
                    "0000011100000;" +
                    "0000011100000;" +
                    "0000011100000;" +
                    "0000011100000;" +
                    "0000011100000;" +
                    "0000011100000;" +
                    "0000000000000;" +
                    "0000000000000;" +
                    "0000000000000;" +
                    "0000000000000;" +
                    "0000000000000;" +
                    "0000000000000" // 5강
            };
        public Skill_008() : base(8)
        {
            if(extension.Length > 0)
            {
                parsedExtension = Common.Extension.Parse<Extension_008>(extension);
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            user.SkillCount--;
            CurrentReuseTime = reuseTime;

            //Strength x 2.0 
            int damage = user.Strength * ParsedExtension.strengthToDamageRatio;

            List<Unit> targetUnits = new List<Unit>();
            foreach (Vector2Int vector in GetRelatePositions(user, target))
            {
                Unit targetUnit = Managers.BattleManager.GetTile(vector).GetUnit();
                if (targetUnit != null)
                {
                    targetUnits.Add(targetUnit);
                }
            }
            user.animationState = Unit.AnimationState.Attack;
            yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);

            foreach (Unit unit in targetUnits)
            {
                Debug.Log($"{user.Name}가 {Name}스킬을 {unit.Name}에 사용!");
                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
                unit.animationState = Unit.AnimationState.Hit;
                // 2단계 : 스킬 적용
                Common.UnitAction.Damage(unit, damage);
                Common.UnitAction.AddEffect(unit, new Effects.Effect_004(unit));
            }
        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = user.Strength * ParsedExtension.strengthToDamageRatio;
            string str = base.GetDescription(user, level).Replace("X", damage.ToString());
            return str;
        }
        public override List<Vector2Int> GetRelatePositions(Unit user, Vector2Int position)
        {
            if (RPSchema == null) return null;

            if (!Common.Range.ParseRangeSchema(APSchema).Contains(position - user.Position)) return null;

            List<Vector2Int> positions = new List<Vector2Int>();

            Vector2Int gap = position - user.Position;
            double angle = Math.Atan2(gap.y, gap.x) - 90 * Math.PI / 180;
            double sin = Math.Sin(-angle);
            double cos = Math.Cos(-angle);
            foreach(Vector2Int vector in Common.Range.ParseRangeSchema(RPSchema))
            {
                Vector2Int pos = new Vector2Int((int)Math.Round((vector.x * cos + vector.y * sin)), (int)Math.Round(vector.x * -sin + vector.y * cos)) + position;
                if (Model.Managers.BattleManager.IsAvilablePosition(pos))
                    positions.Add(pos);
            }
            return positions;
        }
        public override void Upgrade()
        {
            base.Upgrade();
            RPSchema = RPSchemas[Level];
        }
    }

    [System.Serializable]
    public class Extension_008 : Common.Extensionable
    {
        public int strengthToDamageRatio;
    }
}