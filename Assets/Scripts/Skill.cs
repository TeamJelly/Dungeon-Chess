using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillGrade {NULL, Normal, Rare, Legend}
[System.Serializable]
public class Skill
{
    public int number;
    public string name = "No Skill Name";
    public UnitClass unitClass = UnitClass.NULL;
    public SkillGrade skillGrade = SkillGrade.NULL;

    [TextArea(1, 10)]
    public string description;

    public int reuseTime;

    public enum Target {NULL, Me, Single, MultipleFixed, MultipleRotate, Party} 
    public Target target = Target.NULL;

    public List<Vector2Int> AvailablePosition; // 사용가능한 위치
    public List<Vector2Int> MultipleRange; // 다중 범위

    public List<Vector2Int> SelectedPosition; // 사용자가 최종적으로 선택한 위치 범위 (절대 위치)

    public void Use(Unit unit)
    {
        

        // 여기에 구체적인 스킬 사용 구현
    }

    public void Use(List<Unit> units)
    {
        foreach (var unit in units)
            Use(unit);
    }

    public void FindUnit(List<Vector2Int> SelectedPosition)
    {
        List<Unit> tempUnit = null; //= RoomManager.Units; // 있는애만 남김

        foreach (var unit in tempUnit)
            foreach (var position in SelectedPosition)
                if (unit.IsContainPostion(position)) // 있으면
                    Use(unit);
    }
}
