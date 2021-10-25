using Common;
using Common.DB;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public enum SkillCategory
    {
        Null = -1,
        Move,
        Basic,
        Intermediate,
        Advanced,
    }

    [System.Serializable]
    public class Skill : Infoable
    {
        // 스킬 카테고리

        public enum RangeType // 스킬 사용가능 범위의 종류
        {
            Fixed,          // 나를 기준으로 범위가 회전하지 않는 스킬.
            Rotate,         // 나를 기준으로 범위가 회전하는 스킬.
        }

        public enum TargetType // 스킬의 대상
        {
            NULL = -1,
            Any,            // 모든 타일에 사용가능
            NoUnit,         // 유닛이 없는 곳에만 사용가능, 이동 혹은 소환류 스킬에 사용
            Friendly,       // 우호적인 유닛에 사용가능 (AI 용)
            Hostile,        // 적대적인 유닛에 사용가능 (AI 용)
        }

        public SkillCategory Category { get; set; }
        public string Name { get; set; }
        public int [] ReuseTime { get; set; }
        public AI.Priority Priority { get; set; }
        public TargetType Target { get; set; }
        public RangeType Range { get; set; }
        public string [] APData { get; set; }
        public string [] RPData { get; set; }
        public List<UnitSpecies> species = new List<UnitSpecies>();
        public Sprite Sprite { get; set; }
        public Color Color { get; set; }
        public string Description { get; set; }
        public string Type => "Skill";

        /// <summary>
        /// 유닛 레벨을 스킬레벨로 전환시켜준다.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual int GetSLV(Unit user)
        {
            return 0;
        }

        public virtual bool IsUsable(Unit user)
        {
            // 이번턴에 스킬을 사용하지 않고,
            // 이 스킬이 현재 대기중이지 않고,
            // 스킬 레벨이 음수가 아니라면 사용가능하다.
            if (!user.IsSkilled && !user.WaitingSkills.ContainsKey(this) && GetSLV(user) >= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// user가 userPosition에 있을때 스킬사 용가능한 위치들을 반환한다.
        /// </summary>
        /// <param name="user">스킬 사용자</param>
        /// <param name="userPosition">스킬 사용자의 위치</param>
        /// <returns>사용 가능한 스킬 위치들</returns>
        public virtual List<Vector2Int> GetAvailablePositions(Unit user, Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            if (APData == null) return positions;

            foreach (var position in Common.Data.ParseRangeData(APData[GetSLV(user)]))
            {
                Vector2Int abs = userPosition + position;
                Unit unit = BattleManager.GetUnit(abs);

                // 맵밖에 넘어간다면 사용불가
                if (!FieldManager.IsInField(abs))
                    continue;
                // 모든 타일에 사용가능
                if (Target == TargetType.Any)
                    positions.Add(abs);
                // 유닛 없음타일에만 사용가능
                else if (Target == TargetType.NoUnit && unit == null)
                    positions.Add(abs);
                // 우호적인 유닛에 사용 가능
                else if (Target == TargetType.Friendly && unit != null && 
                    ((user.Alliance == UnitAlliance.Party && 
                    (unit.Alliance == UnitAlliance.Party || unit.Alliance == UnitAlliance.Friendly)) ||
                    (user.Alliance == UnitAlliance.Enemy && unit.Alliance == UnitAlliance.Enemy)))
                    positions.Add(abs);
                // 적대적인 유닛에 사용 가능
                else if (Target == TargetType.Hostile && unit != null &&
                    ((user.Alliance == UnitAlliance.Enemy &&
                    (unit.Alliance == UnitAlliance.Party || unit.Alliance == UnitAlliance.Friendly)) ||
                    (user.Alliance == UnitAlliance.Party && unit.Alliance == UnitAlliance.Enemy)))
                    positions.Add(abs);
                // 어디에도 속하지 않으면 false
                else
                    continue;
            }

            return positions;
        }

        /// <summary>
        /// user가 스킬을 현재 위치에서 사용가능한 위치를 반환합니다.
        /// </summary>
        /// <param name="user">스킬 사용자 유닛</param>
        /// <returns>사용가능한 스킬 위치들</returns>
        public virtual List<Vector2Int> GetAvailablePositions(Unit user)
        {
            return GetAvailablePositions(user, user.Position);
        }

        /// <summary>
        /// 스킬이 범위로 영향을 미칠시 그 범위를 보여줍니다.
        /// </summary>
        /// <param name="user">스킬 사용자 유닛</param>
        /// <param name="skillPosition">스킬을 사용하는 위치</param>
        /// <returns>스킬이 영향을 미치는 위치</returns>
        public virtual List<Vector2Int> GetRelatePositions(Unit user, Vector2Int skillPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            if (RPData == null || !GetAvailablePositions(user).Contains(skillPosition)) return positions;

            foreach (var vector in Common.Data.ParseRangeData(RPData[GetSLV(user)]))
            {
                Vector2Int abs = skillPosition + vector;
                if (FieldManager.IsInField(abs))
                    positions.Add(abs);
            }

            return positions;
        }

        public virtual IEnumerator Use(Unit user, Vector2Int target)
        {
            Debug.LogError(Name + " 스킬을 " + target + "에 사용!");
            user.WaitingSkills.Add(this, ReuseTime[GetSLV(user)]);
            yield return null;
        }

        public virtual string GetDescription(Unit user)
        {
            return "스킬 설명";
        }
    }
}