using UnityEngine;
using System.Collections;
using View;

namespace Model.Effects
{

    public class Poison : Effect
    {
        private readonly int damage = 10;

        public int Damage { get => damage; }

        public Poison() : base()
        {
            Name = "독";
            Description = $"턴을 시작할때 체력 10을 차감합니다. 남은 턴 : {TurnCount}";
        }

        public override void OnAdd()
        {
            TurnCount = 3;

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
            FadeOutTextView.MakeText(Owner, $"독! ({TurnCount})", Color.green);
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