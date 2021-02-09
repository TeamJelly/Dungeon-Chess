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

        protected string spritePath;

        private Sprite sprite;
        public Sprite Sprite
        {
            get
            {
                if (sprite == null && spritePath != "")
                    sprite = Resources.Load<Sprite>(spritePath);
                return sprite;
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
            Party,
            Friendly,
            Enemy,
        }

        public Type type = Type.Fixed;          // Default : Fixed
        public Target target = Target.Any;      // Default : Any

        protected string APSchema;
        protected string RPSchema;              // relate Position

        public virtual bool IsAvailablePosition(Unit user, Vector2Int position)
        {
            // 등록한 범위 안이어야 한다.
            if (!Common.Range.ParseRangeSchema(APSchema).Contains(position - user.Position))
                return false;

            // 모든 타일에 사용가능
            if (target == Target.Any)
                return true;
            // 유닛 없음타일에만 사용가능
            else if (target == Target.NoUnit && 
                BattleManager.GetTile(position).IsUsable())
                return true;
            // 파티 유닛에만 사용 가능
            else if (target == Target.Party &&
                BattleManager.GetUnit(position)?.Category == Category.Party)
                return true;
            // 우호적인 유닛에 사용 가능
            else if (target == Target.Friendly && (
                BattleManager.GetUnit(position)?.Category == Category.Friendly ||
                BattleManager.GetUnit(position)?.Category == Category.Party))
                return true;
            // 적대적인 유닛에 사용 가능
            else if (target == Target.Enemy &&
                BattleManager.GetUnit(position)?.Category == Category.Enemy)
                return true;
            // 어디에도 속하지 않으면 false
            else
                return false;
        }

        public virtual bool IsUsable(Unit user)
        {
            if (user.SkillCount > 0 && currentReuseTime == 0)
                return true;
            else
                return false;
        }

        /*public List<Vector2Int> GetAvailablePositions(Unit user)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            foreach (var position in Common.Range.ParseRangeSchema(APSchema))
            {
                Vector2Int abs = user.Position + position;

                if (IsAvailablePosition(user, abs))
                    positions.Add(abs);
            }

            return positions;
        }*/

        // 메인 인디케이터의 위치가 position일때, 관련된 범위의 위치를 돌려줍니다.
        public virtual List<Vector2Int> GetRelatePositions(Unit user, Vector2Int position)
        {
            return Common.Range.ParseRangeSchema(RPSchema);
        }

        public virtual IEnumerator Use(Unit user, Vector2Int target)
        {
            Debug.LogError(name + " 스킬을 " + target + "에 사용!");
            currentReuseTime = reuseTime;
            yield return null;
        }

        public virtual string GetDescription(Unit user)
        {
            string str = description;
            return description;
        }
    }
}