using Common;
using Common.DB;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    using Skills;

    public enum Grade { NULL, Normal, Rare, Legend, Boss }

    [System.Serializable]
    public class Skill
    {
        public int number;                                              // 스킬 번호
        public string name = "No Skill Name";                           // 스킬 이름
        public UnitClass unitClass = UnitClass.NULL;                    // 스킬 클래스
        public Grade grade = Grade.NULL;                                // 스킬 등급

        public AI.Priority priority = AI.Priority.NULL;           // (AI 스킬용) 스킬 타겟 우선순위

        public string spritePath;

        private Sprite sprite;
        public Sprite Sprite
        {
            get
            {
                if (spritePath == "")
                    sprite = Resources.Load<Sprite>("1bitpack_kenney_1/Tilesheet/X");
                else if (sprite == null)
                    sprite = Resources.Load<Sprite>(spritePath);
                return sprite;
            }
        }

        [TextArea(1, 10)]
        public string description;                                      // 스킬 설명
        public int level;                                       // 강화도
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

        public string APSchema;
        public string RPSchema;              // relate Position
        public string extension; // 스킬 고유 특성

        public virtual bool IsUsable(Unit user)
        {
            if (user.SkillCount > 0 && currentReuseTime == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 스킬 사용가능한 절대위치들을 반환한다.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userPosition"></param>
        /// <returns></returns>
        public virtual List<Vector2Int> GetAvailablePositions(Unit user, Vector2Int userPosition)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            if (APSchema == null) return positions;

            foreach (var position in Common.Range.ParseRangeSchema(APSchema))
            {
                Vector2Int abs = userPosition + position;

                // 맵밖에 넘어간다면 사용불가
                if (!BattleManager.IsAvilablePosition(abs))
                    continue;

                // 모든 타일에 사용가능
                if (target == Target.Any)
                    positions.Add(abs);
                // 유닛 없음타일에만 사용가능
                else if (target == Target.NoUnit &&
                    BattleManager.GetTile(abs).HasUnit())
                    positions.Add(abs);
                // 파티 유닛에만 사용 가능
                else if (target == Target.Party &&
                    BattleManager.GetTile(abs).HasUnit() &&
                    BattleManager.GetUnit(abs).Category == Category.Party)
                    positions.Add(abs);
                // 우호적인 유닛에 사용 가능
                else if (target == Target.Friendly &&
                    BattleManager.GetTile(abs).HasUnit() && (
                    BattleManager.GetUnit(abs).Category == Category.Friendly ||
                    BattleManager.GetUnit(abs).Category == Category.Party))
                    positions.Add(abs);
                // 적대적인 유닛에 사용 가능
                else if (target == Target.Enemy &&
                    BattleManager.GetTile(abs).HasUnit() &&
                    BattleManager.GetUnit(abs).Category == Category.Enemy)
                    positions.Add(abs);
                // 어디에도 속하지 않으면 false
                else
                    continue;
            }

            return positions;
        }

        public virtual List<Vector2Int> GetAvailablePositions(Unit user)
        {
            return GetAvailablePositions(user, user.Position);
        }

        // 메인 인디케이터의 위치가 position일때, 관련된 범위의 위치를 돌려줍니다.
        public virtual List<Vector2Int> GetRelatePositions(Unit user, Vector2Int position)
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            if (RPSchema == null) return positions;

            foreach (var vector in Common.Range.ParseRangeSchema(RPSchema))
            {
                Vector2Int abs = position + vector;
                positions.Add(abs);
            }

            return positions;
        }

        public virtual IEnumerator Use(Unit user, Vector2Int target)
        {
            Debug.LogError(name + " 스킬을 " + target + "에 사용!");
            currentReuseTime = reuseTime;
            yield return null;
        }

        /// <summary>
        /// 스킬을 초기화 합니다.
        /// </summary>
        /// <param name="skill_no">스킬번호</param>
        /// <returns></returns>
        protected void InitializeSkillFromDB(int skill_no)
        {
            Skill[] results = new Skill[1];
            results[0] = SkillDictionary.Instance[skill_no];
            // 스킬을 새롭게 DB에서 불러와야하는 경우
            if (results[0] == null)
            {
                results = Query.Instance.SelectFrom<Skill>("skill_table", $"number={skill_no}").results;
                SkillDictionary.Instance[skill_no] = results[0];
            }
            if (results != null && results.Length > 0)
            {
                var skill = results[0];
                number = skill.number;
                name = skill.name;
                unitClass = skill.unitClass;
                grade = skill.grade;
                spritePath = skill.spritePath;
                description = skill.description;
                criticalRate = skill.criticalRate;
                reuseTime = skill.reuseTime;
                type = skill.type;
                target = skill.target;
                APSchema = skill.APSchema;
                RPSchema = skill.RPSchema;
                extension = skill.extension;
            }
            else
            {
                Debug.LogError($"number={skill_no}에 해당하는 스킬이 없습니다.");
            }
        }
        public virtual string GetDescription(Unit user)
        {
            return GetDescription(user, level);
        }
        public virtual string GetDescription(Unit user, int level)
        {
            string str = description;
            return description;
        }

        public virtual void Upgrade()
        {
            level++;
        }

        /// <summary>
        /// 스킬을 초기화 할 때 DB에서 값을 불러옵니다.
        /// </summary>
        /// <param name="no">스킬 번호</param>
        public Skill(int no)
        {
            InitializeSkillFromDB(no);
        }
    }
}