using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Effect : MonoBehaviour
{
    public int number;
    public new string name;
    public Unit currentUnit;

    [TextArea(1, 10)]
    public string description = "효과 설명";

    public void OnGetThisEffect()
    {

    }

    public void OnRemoveThisEffect()
    {

    }

    public void OnBattleStart()
    {

    }

    public void OnBattleEnd()
    {

    }

    public void OnTurnStart()
    {

    }

    public void OnTurnEnd()
    {

    }

    public void BeforeMove()
    {

    }

    public void AfterMove()
    {

    }

    public void BeforeUseSkill()
    {

    }

    public void AfterUseSkill()
    {

    }

    public void BeforeGetDamage()
    {

    }

    public void AfterGetDamamge()
    {

    }

    public void OnGetOtherEffect()
    {

    }

    public void BeforeUseItem()
    {

    }

    public void AfterUseItem()
    {

    }

    public void BeforeUnitDie()
    {

    }
}
