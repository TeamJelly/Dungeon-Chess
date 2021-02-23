using UnityEngine;

namespace Model.Effects
{
    public class Effect_005 : Effect
    {
        private int regen = 10;
        public int Regen
        {
            get => regen;
            set => regen = value;
        }
        public Effect_005(Unit owner, int turnCount) : base(owner, 5)
        {
            TurnCount = turnCount;
        }

        public override void OnAddThisEffect()
        {
            base.OnAddThisEffect();
            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
        }

        public override void OnTurnStart()
        {
            Common.UnitAction.Heal(Owner, Regen);
            TurnCount--;

            Debug.Log($"{Name}효과 {TurnCount}턴 남음");

            if (TurnCount == 0)
            {
                Common.UnitAction.RemoveEffect(Owner, this);
            }
        }
    }
}