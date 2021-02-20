using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Model.Effects
{
    public class Effect_013 : Effect
    {
        public Effect_013(Unit owner, int turnCount) : base(owner)
        {
            descriptor.turnCount = turnCount;
            descriptor.number = 21;
            descriptor.name = "도발 ";
            descriptor.description = "도발의 턴 만큼, 도발을 건 대상에게만 스킬을 사용 할 수 있게 만든다.";
        }
        public override void OnTurnStart()
        {
            descriptor.turnCount--;
            Debug.Log($"{Name}효과 {TurnCount}턴 남음");
            if (descriptor.turnCount == 0) UnitAction.RemoveEffect(Owner, this);
        }
        public override void OnAddThisEffect()
        {
            base.OnAddThisEffect();
            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
        }
    }
}
