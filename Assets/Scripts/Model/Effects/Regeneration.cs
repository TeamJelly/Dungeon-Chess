using UnityEngine;
using View;

namespace Model.Effects
{
    public class Regeneration : Effect
    {
        private readonly int regen = 10;

        public int Regen { get => regen; }

        public Regeneration(Unit owner, int turnCount) : base(owner, turnCount)
        {
            Name = "재생";
            Description = $"턴을 시작할때 체력 10을 회복합니다. 남은 턴 : {turnCount}";
        }

        public override void OnAdd()
        {
            base.OnAdd();
            Owner.OnTurnStart.before.AddListener(OnTurnStart);
            Owner.OnTurnEnd.after.AddListener(OnTurnEnd);
            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
        }
        public override void OnRemove()
        {
            base.OnRemove();
            Owner.OnTurnStart.before.RemoveListener(OnTurnStart);
            Owner.OnTurnEnd.after.RemoveListener(OnTurnEnd);
        }

        public override bool OnTurnStart(bool value)
        {
            FadeOutTextView.MakeText(Owner.Position + Vector2Int.up, $"재생! ({TurnCount})", Color.green);
            Common.Command.Heal(Owner, Regen);

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