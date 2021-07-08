using UnityEditor;
using UnityEngine;

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

            Debug.Log($"{Owner.Name} 기절 상태이상!");
            Owner.MoveCount = 0;
            Owner.SkillCount = 0;

            Owner.OnTurnEnd.after.AddRefListener(OnTurnEnd);
            Owner.OnTurnStart.before.AddRefListener(OnTurnStart);
        }

        public override void OnRemoveThisEffect()
        {
            Owner.OnTurnEnd.after.RemoveRefListener(OnTurnEnd);
            Owner.OnTurnStart.before.RemoveRefListener(OnTurnStart);
        }

        public override void OnTurnStart(ref bool _bool)
        {
            Debug.Log($"{Owner.Name} 기절 상태이상!");
            Owner.MoveCount = 0;
            Owner.SkillCount = 0;
            isActivated = true;
        }

        public override void OnTurnEnd(ref bool _bool)
        {
            if (isActivated == true)
            {
                Common.UnitAction.RemoveEffect(Owner, this);
            }
        }



    }
}