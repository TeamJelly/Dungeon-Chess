using UnityEngine;
using System.Collections;

namespace Model.Effects
{

    public class Effect_021 : Effect
    {
        private int barrierCount;
        public int BarrierCount
        {
            get => barrierCount;
            set => barrierCount = value;
        }

        public Effect_021(Unit owner, int barrierCount, int turnCount) : base(owner, 21)
        {
            TurnCount = turnCount;
            BarrierCount = barrierCount;
        }

        public override int BeforeGetDamage(int damage)
        {
            damage = 0;
            BarrierCount--;
            if (BarrierCount == 0) Common.UnitAction.RemoveEffect(Owner, this);
            return base.BeforeGetDamage(damage);
        }

        public override void OnAddThisEffect()
        {
            base.OnAddThisEffect();
            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
        }

        public override void OnTurnStart()
        {
            TurnCount--;
            Debug.Log($"{Name}효과 {TurnCount}턴 남음");
            if (TurnCount == 0) Common.UnitAction.RemoveEffect(Owner, this);
        }
    }
}