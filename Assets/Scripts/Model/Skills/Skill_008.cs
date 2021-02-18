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
            if (extension != null)
            {
                parsedExtension = Common.Extension.Parse<Extension_008>(extension);
            }
        }
        public override IEnumerator Use(Unit user, Vector2Int target)
        {
            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
            Unit targetUnit = Model.Managers.BattleManager.GetUnit(target);
            user.SkillCount--;
            currentReuseTime = reuseTime;

            int damage = user.Strength * 2;

            if (targetUnit != null)
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {targetUnit.Name}에 사용!");
                user.animationState = Unit.AnimationState.Attack;

                // 2단계 : Acttack 후에 맞는 애니메이션, HP갱신 재생
                yield return new WaitWhile(() => user.animationState != Unit.AnimationState.Idle);
                targetUnit.animationState = Unit.AnimationState.Hit;
                Common.UnitAction.Damage(targetUnit, damage);
                Common.UnitAction.AddEffect(targetUnit, new Effects.Effect_004(targetUnit));
            }
            else
            {
                Debug.Log($"{user.Name}가 {name}스킬을 {target}에 사용!");
            }
            

        }
        public override string GetDescription(Unit user, int level)
        {
            int damage = user.Strength * 2;
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
                Vector2Int pos = new Vector2Int((int)Math.Round((vector.x * cos + vector.y * sin)), (int)Math.Round(vector.x * -sin + vector.y * cos));
                positions.Add(pos + position);
            }
            return positions;
        }
        public override void Upgrade()
        {
            base.Upgrade();
            if(enhancedLevel < 5)
                RPSchema = RPSchemas[enhancedLevel];
        }
    }

    [System.Serializable]
    public class Extension_008 : Common.Extensionable
    {
        public int upgradePerEnhancedLevel;
    }
}
// 추후에 아래 코드를 참고해서 리펙토링 바람
//string[] RPSchemas = {
//        "3;" +
//            "111;" +
//            "111;" +
//            "000", // 0강

//        "5;" +
//            "01110;" +
//            "01110;" +
//            "01110;" +
//            "00000;" +
//            "00000", // 1강

//        "7;" +
//            "0011100;" +
//            "0011100;" +
//            "0011100;" +
//            "0011100;" +
//            "0000000;" +
//            "0000000;" +
//            "0000000", // 2강

//        "9;" +
//            "000111000;" +
//            "000111000;" +
//            "000111000;" +
//            "000111000;" +
//            "000111000;" +
//            "000000000;" +
//            "000000000;" +
//            "000000000;" +
//            "000000000", // 3강

//        "11;" +
//            "00001110000;" +
//            "00001110000;" +
//            "00001110000;" +
//            "00001110000;" +
//            "00001110000;" +
//            "00001110000;" +
//            "00000000000;" +
//            "00000000000;" +
//            "00000000000;" +
//            "00000000000;" +
//            "00000000000", // 4강

//        "13;" +
//            "0000011100000;" +
//            "0000011100000;" +
//            "0000011100000;" +
//            "0000011100000;" +
//            "0000011100000;" +
//            "0000011100000;" +
//            "0000011100000;" +
//            "0000000000000;" +
//            "0000000000000;" +
//            "0000000000000;" +
//            "0000000000000;" +
//            "0000000000000;" +
//            "0000000000000" // 5강
//    };