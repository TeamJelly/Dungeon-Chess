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
        NULL = -1,
        Move,
        Player,
        Enemy
    }

    // [System.Serializable]
    public class Skill : Infoable
    {
        // 스킬 카테고리

        public enum RangeType // 스킬 사용가능 범위의 종류
        {
            Fixed,          // 나를 기준으로 범위가 회전하지 않는 스킬.
            Rotate,         // 나를 기준으로 범위가 회전하는 스킬.
        }

        public enum TargetType // 스킬로 지정할수 있는 대상
        {
            NULL = -1,
            Any,            // 모든 타일에 사용가능
            Posable,        // 위치 가능한 타일에만 사용가능, 이동 혹은 소환류 스킬에 사용
            Friendly,       // 우호적인 유닛에 사용가능 (AI 용)
            Hostile,        // 적대적인 유닛에 사용가능 (AI 용)
        }

        // 스킬에 대한 변수
        public Unit User { get; set; }
        public SkillCategory Category { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int WaitingTime { get; set; } // 현재 스킬 쿨타임
        public int ReuseTime { get; set; } // 재사용 대기시간
        public string APData { get; set; }
        public string RPData { get; set; }

        // 스킬의 속성, 타입
        public AI.Priority Priority { get; set; }
        public TargetType UserTarget { get; set; }
        public TargetType AITarget { get; set; }
        public RangeType Range { get; set; }

        public List<UnitSpecies> species = new List<UnitSpecies>();

        public int SpriteNumber { get; set; }
        public Color InColor { get; set; }
        public Color OutColor { get; set; }
        private Sprite sprite;
        public Sprite Sprite
        {
            get
            {
                if (sprite == null)
                    sprite = Data.MakeSprite(SpriteNumber, InColor, OutColor);
                return sprite;
            }
        }

        public string Description { get; set; }
        public string Type => "Skill";

        public virtual bool IsUsable()
        {
            // 이번턴에 스킬을 사용하지 않고,
            // 이 스킬이 현재 대기중이지 않고,
            // 스킬 레벨이 음수가 아니라면 사용가능하다.
            if (!User.IsSkilled && WaitingTime == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 해당 위치가 실제로 사용가능한지를 제외하고 스킬 사용 위치값만 계산한다.
        /// </summary>
        /// <param name="userPosition"></param>
        /// <returns></returns>
        public virtual List<Vector2Int> GetUseRange(Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            if (APData != null)
            {
                foreach (var position in Common.Data.ParseRangeData(APData))
                {
                    Vector2Int abs = userPosition + position;
                    positions.Add(abs);
                }
            }
            return positions;
        }

        /// <summary>
        /// user가 userPosition에 있을때 스킬 사용가능한 위치들을 반환한다.
        /// </summary>
        /// <param name="userPosition">스킬 사용자의 위치</param>
        /// <returns>사용 가능한 스킬 위치들</returns>
        public virtual List<Vector2Int> GetAvlPositions(Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            TargetType Target = this.UserTarget;

            if ((User.Alliance != UnitAlliance.Party && AITarget != TargetType.NULL) || GameManager.InAuto)
                Target = this.AITarget;

            foreach (var position in GetUseRange(userPosition))
            {
                Unit targetUnit = BattleManager.GetUnit(position);

                // 맵밖에 넘어간다면 사용불가
                if (!FieldManager.IsInField(position))
                    continue;
                // 모든 타일에 사용가능
                if (Target == TargetType.Any)
                    positions.Add(position);
                // 유닛 없음타일에만 사용가능
                else if (Target == TargetType.Posable && FieldManager.GetTile(position).IsPositionable(User))
                    positions.Add(position);
                // 우호적인 유닛에 사용 가능
                else if (Target == TargetType.Friendly && targetUnit != null &&
                    ((User.Alliance == UnitAlliance.Party &&
                    (targetUnit.Alliance == UnitAlliance.Party || targetUnit.Alliance == UnitAlliance.Friendly)) ||
                    (User.Alliance == UnitAlliance.Enemy && targetUnit.Alliance == UnitAlliance.Enemy)))
                    positions.Add(position);
                // 적대적인 유닛에 사용 가능
                else if (Target == TargetType.Hostile && targetUnit != null &&
                    ((User.Alliance == UnitAlliance.Enemy &&
                    (targetUnit.Alliance == UnitAlliance.Party || targetUnit.Alliance == UnitAlliance.Friendly)) ||
                    (User.Alliance == UnitAlliance.Party && targetUnit.Alliance == UnitAlliance.Enemy)))
                    positions.Add(position);
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
        public virtual List<Vector2Int> GetAvlUsePositions()
        {
            return GetAvlPositions(User.Position);
        }

        /// <summary>
        /// 스킬과 관련된 위치를 보여줍니다.
        /// </summary>
        /// <param name="user">스킬 사용자 유닛</param>
        /// <param name="skillPosition">스킬을 사용하는 위치</param>
        /// <returns>스킬이 영향을 미치는 위치</returns>
        public virtual List<Vector2Int> GetRelatePositions(Vector2Int skillPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            // 관련된 범위를 표현하는게 가능하지 않은 경우
            if (RPData == null || !GetAvlUsePositions().Contains(skillPosition)) return positions;

            foreach (var vector in Common.Data.ParseRangeData(RPData))
            {
                Vector2Int abs = skillPosition + vector;
                if (FieldManager.IsInField(abs))
                    positions.Add(abs);
            }

            return positions;
        }

        public virtual IEnumerator Use(Vector2Int target)
        {
            Debug.LogError(Name + " 스킬을 " + target + "에 사용!");

            User.IsSkilled = true;
            WaitingTime = ReuseTime;

            yield return null;
        }

        public virtual void OnUpgrade(int level)
        {
            Level = level;

            // 레벨에 따른 업그레이드를 해줍시다.   
            if (Level == 0)
            {

            }
            else if (Level == 1)
            {

            }
            else if (Level == 2)
            {
                // ... 레벨에 따라 값을 넣어줍니다.
            }
        }

        public virtual string GetDescription()
        {
            return "스킬 설명";
        }

        public Skill Clone() => System.Activator.CreateInstance(GetType()) as Skill;

        public Skill Clone(int level) => System.Activator.CreateInstance(GetType(), level) as Skill;
    }
}