using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Model.Effects
{
    public class E013_Taunt : Effect
    {
        public E013_Taunt(Unit owner, int turnCount) : base(owner, 13)
        {
            Number = 13;
            Name = "도발";
            Description = "도발의 턴 만큼, 도발을 건 대상에게만 스킬을 사용 할 수 있게 만든다.";
        }

        //public override void OnAddThisEffect()
        //{
        //    base.OnAddThisEffect();
        //    Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
        //}

        //public override void OnTurnStart(bool _bool)
        //{
        //    TurnCount--;
        //    Debug.Log($"{Name}효과 {TurnCount}턴 남음");
        //    if (TurnCount == 0) UnitAction.RemoveEffect(Owner, this);
        //}

    }
}
