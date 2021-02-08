using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Model
{
    public class Effect
    {
        public int number;
        public string name;
        public Unit owner;

        [TextArea(1, 10)]
        public string description = "효과 설명";

        public virtual void OnOverlapEffect(Effect newEffect)
        {

        }

        public virtual void OnAddThisEffect()
        {

        }

        public virtual void OnRemoveThisEffect()
        {

        }

        public virtual void OnBattleStart()
        {

        }

        public virtual void OnBattleEnd()
        {

        }

        public virtual void OnTurnStart()
        {

        }

        public virtual void OnTurnEnd()
        {

        }

        public virtual void BeforeMove()
        {

        }

        public virtual void AfterMove()
        {

        }

        public virtual void BeforeUseSkill()
        {

        }

        public virtual void AfterUseSkill()
        {

        }

        public virtual void BeforeGetDamage()
        {

        }

        public virtual void AfterGetDamamge()
        {

        }

        public virtual void OnGetOtherEffect()
        {

        }

        public virtual void BeforeUseItem()
        {

        }

        public virtual void AfterUseItem()
        {

        }

        public virtual void BeforeUnitDie()
        {

        }
    }
}