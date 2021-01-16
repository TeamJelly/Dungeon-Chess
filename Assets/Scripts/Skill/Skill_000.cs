using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 이름: 베기
/// </summary>
public class Skill_000 : Skill
{
    public override void UseSkillToUnit(Unit unit)
    {
        unit.currentHP -= 10;
        base.UseSkillToUnit(unit);
    }

    public override void UseSkillToTile(Vector2Int position)
    {
        Debug.LogError(name + "스킬을" + position + "에 사용!");
        if (BattleManager.instance.AllTiles[position.x, position.y].GetUnit())
            BattleManager.instance.AllTiles[position.x, position.y].GetUnit().currentHP -= 10;
        base.UseSkillToTile(position);
    }
}
