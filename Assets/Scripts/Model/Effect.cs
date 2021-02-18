using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Model
{
    [System.Serializable]
    public class EffectDescriptor
    {
        public int number;
        public string name;
        public Unit owner;
        public string extension;
        [TextArea(1, 10)]
        public string description = "효과 설명";
    }

    public class Effect
    {
        protected EffectDescriptor descriptor = new EffectDescriptor();
        public Unit Owner => descriptor.owner;
        public string Name => descriptor.name;
        public int Number => descriptor.number;
        public string Extension => descriptor.extension;
        public string Description => descriptor.description;

        public Effect()
        {

        }
        public Effect(Unit owner)
        {
            descriptor.owner = owner;
        }
        /// <summary>
        /// 효과의 중복 검사와 중복 처리를 해준다.
        /// </summary>
        public virtual void OnOverlapEffect(Effect oldEffect)
        {
            Owner.StateEffects.Remove(oldEffect);
        }

        public virtual void OnAddThisEffect()
        {
            Effect oldEffect = Common.UnitAction.GetEffectByNumber(Owner, Number);
            if(oldEffect != null)
                OnOverlapEffect(oldEffect);
            Debug.Log($"{Owner.Name}에게 {Name} 효과 추가됨");
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

        public virtual void BeforeUseSkill(Skill skill)
        {

        }

        public virtual void AfterUseSkill()
        {

        }

        public virtual int BeforeGetDamage(int damage)
        {
            return damage;
        }

        public virtual int AfterGetDamamge(int damage)
        {
            return damage;
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