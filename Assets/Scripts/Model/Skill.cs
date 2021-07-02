using Common;
using Common.DB;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [System.Serializable]
    public class Skill
    {
        public enum RangeType // 스킬 사용가능 범위의 종류
        {
            NULL,
            Fixed,          // 나를 기준으로 범위가 회전하지 않는 스킬.
            Rotate,         // 나를 기준으로 범위가 회전하는 스킬.
        }

        public enum TargetType // 스킬의 대상
        {
            NULL,
            Any,            // 모든 타일에 사용가능
            NoUnit,         // 유닛이 없는 곳에만 사용가능, 이동 혹은 소환류 스킬에 사용
            Party,
            Friendly,
            Enemy,
        }
        public string Name { get; set; }
        public int Number           { get; set; }
        public UnitClass UnitClass { get; set; }
        public int Grade            { get; set; }
        public int MaxGrade         { get; set; }
        public int ReuseTime        { get; set; }
        public int CurReuseTime     { get; set; }
        public int CriticalRate     { get; set; }

        public AI.Priority Priority { get; set; }
        public TargetType Target { get; set; }
        public RangeType Range { get; set; }

        [SerializeField]
        protected string spritePath = "";
        public string Description { get; set; }
        public string APData        { get; set; }
        public string RPData        { get; set; }

        private Sprite sprite;
        public Sprite Sprite
        {
            get
            {
                if (spritePath == "")
                    sprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/X");
                else if (sprite == null && spritePath != "")
                    sprite = Resources.Load<Sprite>(spritePath);
                return sprite;
            }
        }

        public virtual bool IsUsable(Unit user)
        {
            if (user.SkillCount > 0 && CurReuseTime == 0)
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

            foreach (var position in Common.Data.ParseRangeData(APData))
            {
                Vector2Int abs = userPosition + position;

                // 맵밖에 넘어간다면 사용불가
                if (!BattleManager.IsAvilablePosition(abs))
                    continue;

                // 모든 타일에 사용가능
                if (Target == TargetType.Any)
                    positions.Add(abs);
                // 유닛 없음타일에만 사용가능
                else if (Target == TargetType.NoUnit &&
                    BattleManager.GetTile(abs).HasUnit())
                    positions.Add(abs);
                // 파티 유닛에만 사용 가능
                else if (Target == TargetType.Party &&
                    BattleManager.GetTile(abs).HasUnit() &&
                    BattleManager.GetUnit(abs).Alliance == UnitAlliance.Party)
                    positions.Add(abs);
                // 우호적인 유닛에 사용 가능
                else if (Target == TargetType.Friendly &&
                    BattleManager.GetTile(abs).HasUnit() && (
                    BattleManager.GetUnit(abs).Alliance == UnitAlliance.Friendly ||
                    BattleManager.GetUnit(abs).Alliance == UnitAlliance.Party))
                    positions.Add(abs);
                // 적대적인 유닛에 사용 가능
                else if (Target == TargetType.Enemy &&
                    BattleManager.GetTile(abs).HasUnit() &&
                    BattleManager.GetUnit(abs).Alliance == UnitAlliance.Enemy)
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

            foreach (var vector in Common.Data.ParseRangeData(RPData))
            {
                Vector2Int abs = skillPosition + vector;
                if (BattleManager.IsAvilablePosition(abs))
                positions.Add(abs);
            }

            return positions;
        }

        public virtual void CalculateDamage(Unit user)
        {

        }

        public virtual IEnumerator Use(Unit user, Vector2Int target)
        {
            Debug.LogError(Name + " 스킬을 " + target + "에 사용!");
            CurReuseTime = ReuseTime;
            yield return null;
        }

        public virtual void Upgrade()
        {
            if (MaxGrade > Grade)
                Grade++;
        }

        public virtual string GetDescription(Unit user)
        {
            return GetDescription(user, Grade);
        }

        public virtual string GetDescription(Unit user, int grade)
        {
            return Description;
        }
    }
}