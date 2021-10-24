using UnityEngine;
using System.Collections;
using View;

namespace Model.Effects
{
    public class Fast : Effect
    {
        public Fast(Unit owner, int turnCount) : base(owner, turnCount)
        {
            
            Name = "신속";
            Description = $"이동거리가 늘어납니다. 남은 턴 : {turnCount}";
        }
       
        public override void OnAdd()
        {
            base.OnAdd();

            Debug.Log("Mobility" + Owner.Mobility);
            Owner.Mobility += 2;
            Debug.Log("Mobility" + Owner.Mobility);
            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");

            Owner.OnTurnStart.before.AddListener(OnTurnStart);
            Owner.OnTurnEnd.before.AddListener(OnTurnEnd);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            Owner.Mobility -= 2;
            Debug.Log("Mobility" + Owner.Mobility);
            Owner.OnTurnStart.before.RemoveListener(OnTurnStart);
            Owner.OnTurnEnd.before.RemoveListener(OnTurnEnd);
        }
        public override bool OnTurnStart(bool value)
        {
            FadeOutTextView.MakeText(Owner, $"+Fast ({TurnCount})", Color.green);
            return value;
        }
        public override bool OnTurnEnd(bool value)
        {
            if (--TurnCount == 0)
                Common.Command.RemoveEffect(Owner, this);
            return value;
        }
    }
}