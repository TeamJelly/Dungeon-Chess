using UnityEditor;
using UnityEngine;

namespace Model.Effects
{
    public class Effect_005 : Effect
    {
        public int turnCount;
        public int regen = 10;

        public Effect_005(Unit owner, int turnCount)
        {
            number = 5;
            name = "재생";
            description = "부여된 턴 동안, 턴 시작시 회복 수치만큼 HP를 회복한다.";
            this.owner = owner;
            this.turnCount = turnCount;
        }

        public override void OnAddThisEffect()
        {
            OnOverlapEffect();
            Debug.Log($"{owner.Name}에게 {name}효과 {turnCount}턴 동안 추가됨");
        }

        public override void OnTurnStart()
        {
            Common.UnitAction.Heal(owner, regen);
            turnCount--;

            Debug.Log($"{name}효과 {turnCount}턴 남음");

            if (turnCount == 0)
            {
                Common.UnitAction.RemoveEffect(owner, this);
            }
        }
    }
}