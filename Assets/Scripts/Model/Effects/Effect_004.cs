using UnityEditor;
using UnityEngine;

namespace Model.Effects
{
    public class Effect_004 : Effect
    {
        public Effect_004(Unit owner): base(owner)
        {
            descriptor.number = 4;
            descriptor.name = "기절";
            descriptor.description = "한 턴동안 아무것도 하지 못하는 상태가 된다. 턴이 돌아오면 자동으로 턴 종료가 되고" +
                " 턴 시작시 상태이상과 턴 종료시의 상태이상은 모두 적용된다.";
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