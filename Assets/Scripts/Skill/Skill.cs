using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Grade { NULL, Normal, Rare, Legend, Boss }

[System.Serializable]
public class Skill : MonoBehaviour
{
    public int number;                                              // 스킬 번호
    public new string name = "No Skill Name";                       // 스킬 이름
    public UnitClass unitClass = UnitClass.NULL;                    // 스킬 클래스
    public Grade grade = Grade.NULL;                                // 스킬 등급
//    public enum skillImage; 

    [TextArea(1, 10)]
    public string description;                                      // 스킬 설명
    public int enhancedLevel;                                       // 강화도
    public int reuseTime;                                           // 재사용 대기시간

    [Range(0, 100)]
    public float criticalRate;                                      // 크리티컬율

    public enum Target {
        NULL,
        Me, 
        Single, 
        MultipleFixed,
        MultipleRotate
    }

    public Target target = Target.NULL;                             // 대상

    public List<Vector2Int> AvailablePosition;                      // 사용가능한 위치
    public List<Vector2Int> MultipleRange;                          // 다중 범위

    public void UseSkillToUnit(Unit unit)
    {
        // 스킬당 구현 필요
    }

    public void UseSkillToUnit(List<Unit> units)
    {
        foreach (var unit in units)
            UseSkillToUnit(unit);
    }

    public void FindUnit(List<Vector2Int> SelectedPosition)
    {
        List<Unit> tempUnit = null; //= RoomManager.Units; // 있는애만 남김

        foreach (var unit in tempUnit)
            foreach (var position in SelectedPosition)
                if (unit.IsContainPostion(position)) // 있으면
                    UseSkillToUnit(unit);
    }

    public void ShowSkillChoices(Unit user)
    {
        if(target == Target.Me) // 선택지 없음 바로 실행
        {
            UseSkillToUnit(user);
        }
        else if(target == Target.Single)
        {
            BattleUI.instance.ShowTile(AvailablePosition);
        }
        else if(target == Target.MultipleFixed)
        {

        }
    }
    /*
    public List<Vector2Int> GetSkillablePosition(Unit skillUser)
    {
        List<Vector2Int> tiles;

        if (target == Target.Me)
        {
            tiles = UnitPosition.UnitPositionToVector2IntList(skillUser.unitPosition);
        }
        else if (target == Target.Single)
        {
            foreach (var item in AvailablePosition)
            {
//                UnitPosition = skillUser.unitPosition;
                for (int i = 0; i < item.x; i++)
                {
                }
            } 
        }
        
//        return tiles;
    }*/

}