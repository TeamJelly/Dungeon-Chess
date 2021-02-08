using UnityEditor;
using UnityEngine;

namespace Model.Effects
{
    public class Effect_004 : Effect
    {
        Effect_004(Unit owner)
        {
            number = 4;
            name = "기절";
            description = "한 턴동안 아무것도 하지 못하는 상태가 된다. 턴이 돌아오면 자동으로 턴 종료가 되고" +
                " 턴 시작시 상태이상과 턴 종료시의 상태이상은 모두 적용된다.";
            this.owner = owner;
        }

        public override void OnOverlapEffect(Effect newEffect)
        {

        }

        public override void OnAddThisEffect()
        {
        }

        public override void OnTurnStart()
        {
            Debug.LogError($"{owner.Name} 기절 상태이상!");
            owner.MoveCount = 0;
            owner.SkillCount = 0;
            owner.ItemCount = 0;

            Common.UnitAction.RemoveEffect(owner, this);
        }

    }
}