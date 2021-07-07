using UnityEngine;

namespace Model.Effects
{
    public class E005_Regeneration : Effect
    {
        private readonly int regen = 10;

        public int Regen { get => regen; }

        public E005_Regeneration(Unit owner, int turnCount) : base(owner, turnCount)
        {
            Number = 5;
            Name = "재생";
            Description = $"턴을 시작할때 체력 10을 회복합니다. 남은 턴 : {turnCount}";
        }

        public override void OnAddThisEffect()
        {
            base.OnAddThisEffect();

            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
        }

        public override void OnTurnStart(ref bool _bool)
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