using UnityEditor;
using UnityEngine;
using View;

namespace Model.Effects
{
    public class E004_Stun : Effect
    {
        public E004_Stun(Unit owner) : base(owner)
        {
            Number = 4;
            Name = "기절";
            Description = "유닛이 한 턴동안 행동하지 못합니다.";
        }

        // 이미 기절 효과가 발동 됨을 기록한다.
        bool isActivated = false;

        public override void OnAddThisEffect()
        {
            base.OnAddThisEffect();

            Owner.IsMoved = true;
            Owner.IsSkilled = true;

            Owner.OnTurnEnd.after.AddListener(OnTurnEnd);
            Owner.OnTurnStart.before.AddListener(OnTurnStart);
        }

        public override void OnRemoveThisEffect()
        {
            Owner.OnTurnEnd.after.RemoveListener(OnTurnEnd);
            Owner.OnTurnStart.before.RemoveListener(OnTurnStart);
        }

        public override bool OnTurnStart(bool value)
        {
            FadeOutTextView.MakeText(Owner.Position + Vector2Int.up, $"기절!", Color.red);

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