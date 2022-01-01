using UnityEditor;
using UnityEngine;
using View;

namespace Model.Effects
{
    public class Stun : Effect
    {
        public Stun() : base()
        {
            Name = "기절";
            Description = "유닛이 한 턴동안 행동하지 못합니다.";
        }

        // 이미 기절 효과가 발동 됨을 기록한다.
        bool isActivated = false;

        public override void OnAdd()
        {
            Owner.IsMoved = true;
            Owner.IsSkilled = true;

            Owner.OnTurnStart.before.AddListener(OnTurnStart);
            Owner.OnTurnEnd.after.AddListener(OnTurnEnd);
        }

        public override void OnRemove()
        {
            Owner.OnTurnStart.before.RemoveListener(OnTurnStart);
            Owner.OnTurnEnd.after.RemoveListener(OnTurnEnd);
        }

        public override bool OnTurnStart(bool value)
        {
            FadeOutTextView.MakeText(Owner, $"+Stun", Color.red);

            Owner.IsMoved = true;
            Owner.IsSkilled = true;
            isActivated = true;

            return value;
        }

        public override bool OnTurnEnd(bool value)
        {
            if (isActivated)
                Common.Command.RemoveEffect(Owner, this);

            return value;
        }



    }
}