//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Model.Skills
//{
//    public class Skill_031 : Skill
//    {
//        private Extension_031 parsedExtension;
//        public Extension_031 ParsedExtension => parsedExtension;
//        public Skill_031() : base(31)
//        {
//            if (extension != null)
//            {
//                parsedExtension = Common.Extension.Parse<Extension_031>(extension);
//            }
//        }
//        public override IEnumerator Use(Unit user, Vector2Int target)
//        {
//            // 0 단계 : 로그 출력, 스킬 소모 기록, 필요 변수 계산
//            user.SkillCount--;
//            CurrentReuseTime = reuseTime;

//            //공격력 계수 1.0 + 강화 횟수 만큼 회복
//            int heal = user.Strength * ParsedExtension.strengthToDamageRatio + Level;

//            Unit targetUnit = Managers.BattleManager.GetUnit(target);
//            if (targetUnit != null)
//            {
//                Debug.Log($"{user.Name}가 {Name}스킬을 {targetUnit.Name}에 사용!");

//                // 1단계 : 스킬 애니메이션 재생 및 화면 갱신.
//                yield return null;

//                // 2단계 : 스킬 적용.
//                Common.UnitAction.Heal(targetUnit, heal);
//            }
//            else
//            {
//                Debug.Log($"{user.Name}가 {Name}스킬을 {target}에 사용!");
//            }


//        }
//        public override string GetDescription(Unit user, int level)
//        {
//            int heal = user.Strength * ParsedExtension.strengthToDamageRatio + level;
//            string str = base.GetDescription(user, level).Replace("X", heal.ToString());
//            return str;
//        }
//    }
//    [System.Serializable]
//    public class Extension_031 : Common.Extensionable
//    {
//        public int strengthToDamageRatio;
//    }
//}

