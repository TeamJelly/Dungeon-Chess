using UnityEngine;
using System.Collections;
using View;

namespace Model.Effects
{

    public class Poison : Effect
    {
        private readonly int damage = 10;

        public int Damage { get => damage; }

        public Poison(Unit owner, int turnCount) : base(owner, turnCount)
        {
            Number = 5;
            Name = "독";
            Description = $"턴을 시작할때 체력 10을 차감합니다. 남은 턴 : {turnCount}";
        }

        public override void OnAdd()
        {
            base.OnAdd();

            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
            Owner.OnTurnStart.before.AddListener(OnTurnStart);
            Owner.OnTurnEnd.before.AddListener(OnTurnEnd);
        }
        public override void OnRemove()
        {
            base.OnRemove();
            Owner.OnTurnStart.before.RemoveListener(OnTurnStart);
            Owner.OnTurnEnd.before.RemoveListener(OnTurnEnd);

        }
        public override bool OnTurnStart(bool value)
        {
            FadeOutTextView.MakeText(Owner.Position + Vector2Int.up, $"독! ({TurnCount})", Color.green);
            Common.Command.Damage(Owner, Damage);

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