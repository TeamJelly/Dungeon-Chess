// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Model.Skills
// {
//     public class S008_PowerWave : Skill
//     {
//         public S008_PowerWave()
//         {
//             Name = "힘의파동";
//             Number = 8;
//             MaxLevel = 5;
//             ReuseTime = 4;
//             CriticalRate = 5;

//             Priority = Common.AI.Priority.NULL;
//             Target = TargetType.Any;
//             Range = RangeType.Rotate;

//             Sprite = Common.Data.LoadSprite("HandMade/SkillImage/008_힘의 파동");
//             Description =
//                 $"범위 안에 있는 대상에게 X 데미지를 주고 기절시킨다.\n\n\n " +
//                 $"‘  “엑스...칼리버!!! “.’";

//             APData = "3;010;101;010";
//         }

//         private readonly int strToDmg = 2;

//         string[] RPDatas = {
//                 "3;" +
//                     "111;" +
//                     "111;" +
//                     "000", // 0강

//                 "5;" +
//                     "01110;" +
//                     "01110;" +
//                     "01110;" +
//                     "00000;" +
//                     "00000", // 1강

//                 "7;" +
//                     "0011100;" +
//                     "0011100;" +
//                     "0011100;" +
//                     "0011100;" +
//                     "0000000;" +
//                     "0000000;" +
//                     "0000000", // 2강

//                 "9;" +
//                     "000111000;" +
//                     "000111000;" +
//                     "000111000;" +
//                     "000111000;" +
//                     "000111000;" +
//                     "000000000;" +
//                     "000000000;" +
//                     "000000000;" +
//                     "000000000", // 3강

//                 "11;" +
//                     "00001110000;" +
//                     "00001110000;" +
//                     "00001110000;" +
//                     "00001110000;" +
//                     "00001110000;" +
//                     "00001110000;" +
//                     "00000000000;" +
//                     "00000000000;" +
//                     "00000000000;" +
//                     "00000000000;" +
//                     "00000000000", // 4강

//                 "13;" +
//                     "0000011100000;" +
//                     "0000011100000;" +
//                     "0000011100000;" +
//                     "0000011100000;" +
//                     "0000011100000;" +
//                     "0000011100000;" +
//                     "0000011100000;" +
//                     "0000000000000;" +
//                     "0000000000000;" +
//                     "0000000000000;" +
//                     "0000000000000;" +
//                     "0000000000000;" +
//                     "0000000000000" // 5강
//             };

//         public override IEnumerator Use(Unit user, Vector2Int target)
//         {
//             // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
//             user.IsSkilled = true;
//             user.WaitingSkills[this] = ReuseTime;

//             //Strength x 2.0 
//             int damage = user.Strength * strToDmg;

//             List<Unit> targetUnits = new List<Unit>();
//             foreach (Vector2Int vector in GetRelatePositions(user, target))
//             {

//                 View.VisualEffectView.MakeVisualEffect(vector, "Explosion");

//                 Unit targetUnit = Managers.FieldManager.GetTile(vector).GetUnit();
//                 if (targetUnit != null)
//                 {
//                     targetUnits.Add(targetUnit);
//                 }
//             }

//             foreach (Unit unit in targetUnits)
//             {
//                 Debug.Log($"{user.Name}가 {Name}스킬을 {unit.Name}에 사용!");
//                 // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
//                 yield return null;
//                 // 2단계 : 스킬 적용
//                 Common.Command.Damage(unit, damage);
//                 Common.Command.AddEffect(unit, new Effects.E004_Stun(unit));
//             }
//         }

//         public override string GetDescription(Unit user, int level)
//         {
//             int damage = user.Strength * strToDmg;
//             string str = base.GetDescription(user, level).Replace("X", damage.ToString());
//             return str;
//         }

//         public override List<Vector2Int> GetRelatePositions(Unit user, Vector2Int position)
//         {
//             if (RPData == null) return null;

//             if (!Common.Data.ParseRangeData(APData).Contains(position - user.Position)) return null;

//             List<Vector2Int> positions = new List<Vector2Int>();

//             Vector2Int gap = position - user.Position;
//             double angle = Math.Atan2(gap.y, gap.x) - 90 * Math.PI / 180;
//             double sin = Math.Sin(-angle);
//             double cos = Math.Cos(-angle);
//             foreach (Vector2Int vector in Common.Data.ParseRangeData(RPData))
//             {
//                 Vector2Int pos = new Vector2Int((int)Math.Round((vector.x * cos + vector.y * sin)), (int)Math.Round(vector.x * -sin + vector.y * cos)) + position;
//                 if (Managers.FieldManager.IsInField(pos))
//                     positions.Add(pos);
//             }
//             return positions;
//         }

//         public override void Upgrade()
//         {
//             base.Upgrade();
//             RPData = RPDatas[Level];
//         }
//     }
// }