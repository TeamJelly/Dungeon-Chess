using UnityEditor;
using UnityEngine;

namespace Model.Effects
{
    public class Effect_004 : Effect
    {
        public Effect_004(Unit owner): base(owner, 4) 
        {
            TurnCount = 1;
        }

        bool isActivated = false;

        public override void OnTurnStart()
        {
            Debug.Log($"{Owner.Name} 기절 상태이상!");
            Owner.MoveCount = 0;
            Owner.SkillCount = 0;
            Owner.ItemCount = 0;
            isActivated = true;
        }

        public override void OnAddThisEffect()
        {
            base.OnAddThisEffect();
            Debug.Log($"{Owner.Name} 기절 상태이상!");
            Owner.MoveCount = 0;
            Owner.SkillCount = 0;
            Owner.ItemCount = 0;
        }

        public override void OnTurnEnd()
        {
            if (isActivated == true)
                Common.UnitAction.RemoveEffect(Owner, this);
        }

    }
}