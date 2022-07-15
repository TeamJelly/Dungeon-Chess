using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill : MonoBehaviour, ISpriteable
{
    /// <summary>
    /// 스킬 카테고리
    /// </summary>
    public enum SkillCategory
    {
        NULL = -1,
        Move,
        Player,
        Enemy
    }

    /// <summary>
    /// 스킬 사용가능 범위의 종류
    /// </summary>
    public enum RangeEnum
    {
        Fixed,          // 나를 기준으로 범위가 회전하지 않는 스킬.
        Rotate,         // 나를 기준으로 범위가 회전하는 스킬.
    }

    /// <summary>
    /// 스킬로 지정할수 있는 대상
    /// </summary>
    public enum TargetEnum
    {
        NULL = -1,
        Any,            // 모든 타일에 사용가능
        Posable,        // 위치 가능한 타일에만 사용가능, 이동 혹은 소환류 스킬에 사용
        Friendly,       // 우호적인 유닛에 사용가능 (AI 용)
        Hostile,        // 적대적인 유닛에 사용가능 (AI 용)
    }

    // 스킬에 대한 변수
    [SerializeField] private Sprite sprite;
    [SerializeField] private SkillCategory category;
    [SerializeField] private TargetEnum user_target;
    [SerializeField] private TargetEnum ai_target;
    [SerializeField] private RangeEnum range_type;
    [SerializeField] private int reuse_time;
    [SerializeField] private string avilable_position;
    [SerializeField] private string range_position;
    [SerializeField] private string next_skill;

    public string Name { get => name; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public SkillCategory Category { get => category; set => category = value; }
    public int ReuseTime { get => reuse_time; set => reuse_time = value; }
    public TargetEnum UserTarget { get => user_target; set => user_target = value; }
    public TargetEnum AITarget { get => ai_target; set => ai_target = value; }
    public RangeEnum RangeType { get => range_type; set => range_type = value; }
    public string NextSkill { get => next_skill; set => next_skill = value; }

    public virtual bool IsUsable(Unit user)
    {
        // 이번턴에 스킬을 사용하지 않고,
        // 이 스킬이 현재 대기중이지 않고,
        // 스킬 레벨이 음수가 아니라면 사용가능하다.
        if (user.IsSkillable && user.SkillWaitingTime[name] == 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 해당 위치가 실제로 사용가능한지를 제외하고 스킬 사용 위치값만 계산한다.
    /// </summary>
    /// <param name="virtualPosition"></param>
    /// <returns></returns>
    public virtual SortedSet<Vector2Int> GetUseRange(Vector2Int virtualPosition)
    {
        SortedSet<Vector2Int> positions = new SortedSet<Vector2Int>();

        if (avilable_position != null)
        {
            foreach (var position in Converter.ParseRangeData(avilable_position))
            {
                Vector2Int abs = virtualPosition + position;
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
    public virtual SortedSet<Vector2Int> GetAvlPositions(Unit user)
    {
        SortedSet<Vector2Int> positions = new SortedSet<Vector2Int>();

        foreach (Vector2Int position in GetUseRange(user.Position))
            if (EstimatePositionScore(user, position) > 0)
                positions.Add(position);

        return positions;
    }

    /// <summary>
    /// 스킬 사용시 해당 위치가 얼마나 사용하기 좋은지 알려주는 함수. 음수는 사용불가 위치를 나타냄.
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public virtual int EstimatePositionScore(Unit user, Vector2Int position)
    {
        TargetEnum Target = this.UserTarget;

        if ((user.Alliance != UnitAlliance.Party && AITarget != TargetEnum.NULL) /*|| GameManager.InAuto*/)
            Target = this.AITarget;

        // Unit targetUnit = BattleManager.GetUnit(position);

        // // 맵밖에 넘어간다면 사용불가
        // if (!FieldManager.IsInField(position))
        //     return -1;

        // // 모든 타일에 사용가능
        // else if (Target == TargetEnum.Any)
        //     return 1;

        // // 유닛 없음타일에만 사용가능
        // else if (Target == TargetEnum.Posable && FieldManager.GetTile(position).IsPositionable(user))
        //     return 1;

        // // 우호적인 유닛에 사용 가능
        // else if (Target == TargetEnum.Friendly && targetUnit != null &&
        //     ((user.Alliance == UnitAlliance.Party && (targetUnit.Alliance == UnitAlliance.Party || targetUnit.Alliance == UnitAlliance.Friendly)) ||
        //     (user.Alliance == UnitAlliance.Enemy && targetUnit.Alliance == UnitAlliance.Enemy)))
        //     return 1;

        // // 적대적인 유닛에 사용 가능
        // else if (Target == TargetEnum.Hostile && targetUnit != null &&
        //     ((user.Alliance == UnitAlliance.Enemy && (targetUnit.Alliance == UnitAlliance.Party || targetUnit.Alliance == UnitAlliance.Friendly)) ||
        //     (user.Alliance == UnitAlliance.Party && targetUnit.Alliance == UnitAlliance.Enemy)))
        //     return 1;

        // 어디에도 속하지 않으면 false
        // else
        return -1;
    }


    /// <summary>
    /// 스킬과 관련된 위치를 보여줍니다.
    /// </summary>
    /// <param name="user">스킬 사용자 유닛</param>
    /// <param name="skillPosition">스킬을 사용하는 위치</param>
    /// <returns>스킬이 영향을 미치는 위치</returns>
    public virtual SortedSet<Vector2Int> GetRelatePositions(Unit user, Vector2Int skillPosition)
    {
        SortedSet<Vector2Int> positions = new SortedSet<Vector2Int>();

        // 관련된 범위를 표현하는게 가능하지 않은 경우
        if (range_position == null || !GetAvlPositions(user).Contains(skillPosition)) return positions;

        foreach (var vector in Converter.ParseRangeData(range_position))
        {
            Vector2Int abs = skillPosition + vector;
            // if (FieldManager.IsInField(abs))
            positions.Add(abs);
        }

        return positions;
    }

    public virtual void Use(Unit user, Vector2Int target)
    {
        Debug.Log(Name + " 스킬을 " + target + "에 사용!");

        if (++user.SkillCount > user.Action)
            user.IsSkillable = false;
        user.SkillWaitingTime[name] = reuse_time;
    }

    public virtual string GetDescription()
    {
        return "스킬 설명";
    }
}