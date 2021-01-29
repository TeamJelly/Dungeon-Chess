using Common;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public enum Grade { NULL, Normal, Rare, Legend, Boss }
    
    [System.Serializable]
    public class Skill
    {
        public int number;                                              // 스킬 번호
        public string name = "No Skill Name";                           // 스킬 이름
        public UnitClass unitClass = UnitClass.NULL;                    // 스킬 클래스
        public Grade grade = Grade.NULL;                                // 스킬 등급

        protected string skillImagePath;
        private Sprite skillImage;

        public Sprite SkillImage
        {
            get
            {
                if (skillImage == null && skillImagePath != "")
                    skillImage = Resources.Load<Sprite>(skillImagePath);
                return skillImage;
            }
        }

        [TextArea(1, 10)]
        public string description;                                      // 스킬 설명
        public int enhancedLevel;                                       // 강화도
        public int reuseTime;                                           // 재사용 대기시간
        public int currentReuseTime;

        [Range(0, 100)]
        public float criticalRate;                                      // 크리티컬율

        public enum Type                                              // 스킬 사용가능 범위의 종류
        {
            NULL,
            Fixed,          // 나를 기준으로 범위가 회전하지 않는 스킬.
            Rotate,         // 나를 기준으로 범위가 회전하는 스킬.
        }

        public enum Target                                              // 스킬의 대상
        {
            NULL,
            Any,            // 모든 타일에 사용가능
            NoUnit,         // 유닛이 없는 곳에만 사용가능, 이동 혹은 소환류 스킬에 사용
        }

        public Type type = Type.Fixed;          // Default : Fixed
        public Target target = Target.Any;      // Default : Any

        protected string APSchema;
        protected string RPSchema;

        public virtual List<Vector2Int> GetAvailablePositions(Unit user)
        {
            return Common.Range.ParseRangeSchema(APSchema);
        }

        public virtual List<Vector2Int> GetRangePositions(Unit user)
        {
            return Common.Range.ParseRangeSchema(RPSchema);
        }

        /// <summary>
        /// 스킬 사용가능한 위치인가?
        /// </summary>
        /// <param name="user"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual bool IsPossible(Unit user, Vector2Int position)
        {
            Vector2Int temp = position - user.position;

            if (GetAvailablePositions(user).Contains(temp))
            {
                if (target == Target.Any)
                    return true;
                else if (target == Target.NoUnit && BattleManager.instance.GetUnit(position) == null)
                    return true;
            }

            return false;
        }

        public virtual void Use(Unit user, Vector2Int target)
        {
            Debug.LogError(name + " 스킬을 " + target + "에 사용!");
            currentReuseTime = reuseTime;
        }

        public virtual void Use(Unit user, List<Vector2Int> targets)
        {
            foreach (var target in targets)
                Use(user, target);
        }
    }
}