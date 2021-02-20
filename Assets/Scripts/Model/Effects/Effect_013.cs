using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Model.Effects
{
    public class Effect_013 : Effect
    {
        public Effect_013(Unit owner, int turnCount) : base(owner, 13)
        {
            TurnCount = turnCount;
        }

        public override void OnTurnStart()
        {
            TurnCount--;
            Debug.Log($"{Name}효과 {TurnCount}턴 남음");
            if (TurnCount == 0) UnitAction.RemoveEffect(Owner, this);
        }
        public override void OnAddThisEffect()
        {
            base.OnAddThisEffect();
            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
        }
    }
}
