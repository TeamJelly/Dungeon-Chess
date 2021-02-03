using Common;
using Common.DB;
using Model.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Grade { NULL, Normal, Rare, Legend, Boss }



namespace Model
{
    [System.Serializable]
    public class Skill
    {
        public int number;                                              // 스킬 번호
        public new string name = "No Skill Name";                       // 스킬 이름
        public UnitClass unitClass = UnitClass.NULL;                    // 스킬 클래스
        public Grade grade = Grade.NULL;                                // 스킬 등급
        public string skillImagePath;

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

        public enum Domain                                              // 스킬 사용가능 범위의 종류
        {
            NULL,
            Me,             // 나에게 사용, Unit
            RandomOne,      // 범위 내 대상에서 랜덤한 하나를 뽑는다.
            SelectOne,      // 범위 내 대상중에서 하나를 선택한다.
            Fixed,          // 복수 대상일 경우 범위가 고정된다.
            Rotate,         // 복수 대상일 경우 범위가 회전한다.
        }

        public enum Target                                              // 스킬의 대상
        {
            NULL,
            AnyTile,            // 타일을 대상으로 사용가능
            NoUnitTile,         // 유닛이 없는 곳에만 사용가능, 소환류 스킬에 사용
            AnyUnit,            // 적, 아군 구별 없이 사용가능
            PartyUnit,          // 내 파티에게만 사용가능
            FriendlyUnit,       // 아군에게만 사용가능
            EnemyUnit,          // 적에게만 사용가능
        }

        public Domain domain = Domain.NULL;
        public Target target = Target.NULL;

        //    private List<Vector2Int> AvailablePositions = new List<Vector2Int>(); // 사용가능한 위치
        //    private List<Vector2Int> RangePositions = new List<Vector2Int>(); // 다중 범위
        public string APSchema;
        public string RPSchema;
        public string extension; // 스킬 고유 특성

        public virtual List<Vector2Int> GetAvailablePositions()
        {
            return Common.Range.ParseRangeSchema(APSchema);
        }

        public virtual List<Vector2Int> GetRangePositions()
        {
            // 스킬 레벨에 따라 달라지는 다중 범위면 변경 필요.
            return Common.Range.ParseRangeSchema(RPSchema);
        }

        public virtual void EnhanceSkill(int level) // 강화할때마다 호출해서 스팩을 업데이트한다.
        {
            enhancedLevel = level;
        }

        public virtual void UseSkillToTile(Tile tile)
        {
            currentReuseTime = reuseTime;
        }

        public virtual void UseSkillToTile(List<Tile> tiles)
        {
            foreach (var tile in tiles)
                UseSkillToTile(tile);
        }

        public virtual void UseSkillToUnit(Unit owner, Unit unit)
        {
            // 스킬당 구현 필요
            currentReuseTime = reuseTime;
        }

        public virtual void UseSkillToUnit(Unit owner, List<Unit> units)
        {
            foreach (var unit in units)
                UseSkillToUnit(owner, unit);
        }

        /*public List<Unit> GetUnitsInDomain()
        {
            Unit skillUser = GetComponent<Unit>();
            return GetUnitsInDomain(skillUser);
        }*/

        public List<Unit> GetUnitsInDomain(Unit skillUser) //
        {
            List<Unit> units = new List<Unit>();

            if (domain == Domain.Me)
                units.Add(skillUser);
            else if (domain == Domain.SelectOne || domain == Domain.RandomOne)
                units = GetUnitsInSelectOne(skillUser);

            return units;
        }


        List<Unit> GetUnitsInSelectOne(Unit skillUser)
        {
            List<Unit> units = new List<Unit>();
            List<Unit> tempUnits = BattleManager.GetUnitsInPositions(GetPositionsInDomain(skillUser)); // 스킬 도메인을 받고 그위의 유닛들을 반환

            foreach (var unit in tempUnits)
            {
                if (target == Target.AnyUnit)
                    units.Add(unit);
                else if (target == Target.EnemyUnit && unit.category == Category.Enemy)
                    units.Add(unit);
                else if (target == Target.FriendlyUnit && (unit.category == Category.Friendly || unit.category == Category.Party))
                    units.Add(unit);
                else if (target == Target.PartyUnit && unit.category == Category.Party)
                    units.Add(unit);
            }

            return units;
        }

        /*public List<UnitPosition> GetUnitPositionsInDomain()
        {
            Unit skillUser = GetComponent<Unit>();
            return GetUnitPositionsInDomain(skillUser);
        }*/

        public List<UnitPosition> GetUnitPositionsInDomain(Unit skillUser)
        {
            List<UnitPosition> unitPositions = new List<UnitPosition>();

            foreach (var item in GetUnitsInDomain(skillUser))
            {
                unitPositions.Add(item.unitPosition);
            }

            return unitPositions;
        }

        /*    public List<Vector2Int> GetPositionsInDomain()
            {
                Unit skillUser = GetComponent<Unit>();
                return GetPositionsInDomain(skillUser);
            }*/

        public List<Vector2Int> GetPositionsInDomain(Unit skillUser) // 스킬 범위를 절대 위치로 바꾼다.
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            foreach (var item in GetAvailablePositions())
            {

                foreach (var position in skillUser.unitPosition.GetAdd(item).GetPositions())
                {
                    if (position.x < 0 || position.y < 0) // 범위 제한
                        continue;
                    if (position.x >= BattleManager.instance.AllTiles.GetLength(1) || position.y >= BattleManager.instance.AllTiles.GetLength(0)) // 범위 제한
                        continue;
                    if (target == Target.NoUnitTile && !BattleManager.instance.AllTiles[position.x, position.y].IsUsable())
                        continue;
                    if (!positions.Contains(position))
                        positions.Add(position);
                }
            }
            return positions;
        }

        public bool IsUsable()
        {
            if (currentReuseTime == 0)
                return true;
            else
                return false;
        }

        /*    public static Tile RandomPick(List<Tile> tiles)
            {
                return tiles[Random.Range(0, tiles.Count)];
            }

            public static Unit RandomPick(List<Unit> units)
            {
                return units[Random.Range(0, units.Count)];
            }*/

        /// <summary>
        /// 스킬을 초기화 합니다.
        /// </summary>
        /// <param name="skill_no">스킬번호</param>
        /// <returns></returns>
        protected void InitializeSkillFromDB(int skill_no)
        {
            var results = Query.Instance.SelectFrom<Skill>("skill_table", $"number={skill_no}").results;
            if (results.Length > 0)
            {
                var skill = results[0];
                number = skill.number;
                name = skill.name;
                unitClass = skill.unitClass;
                grade = skill.grade;
                skillImagePath = skill.skillImagePath;
                description = skill.description;
                criticalRate = skill.criticalRate;
                reuseTime = skill.reuseTime;
                domain = skill.domain;
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
        /// <summary>
        /// 스킬 고유의 확장 스탯을 파싱합니다.
        /// </summary>
        /// <typeparam name="T">확장 클래스</typeparam>
        /// <param name="extension">확장용 스키마를 넣습니다</param>
        /// <returns>확장 클래스</returns>
        protected T ParseExtension<T>(string extension)
        {
            string[] stats = extension.Split(';');
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (var stat in stats)
            {
                var stat_split = stat.Split('=');
                var name = stat_split[0];
                var value = stat_split[1];
                dict.Add(name, value);
            }
            
            string jsonString = JSON.DictionaryToJsonString(dict);
            return JSON.ParseString<T>(jsonString);
        }
    }
}