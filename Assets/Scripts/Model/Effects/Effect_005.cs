using UnityEditor;
using UnityEngine;

namespace Model.Effects
{
    public class Effect_005 : Effect
    {
        public int turnCount;
        public int regen;

        public Effect_005(Unit owner, int turnCount, int regen)
        {
            number = 5;
            name = "재생";
            description = "부여된 턴 동안, 턴 시작시 회복 수치만큼 HP를 회복한다.";
            this.owner = owner;

            this.turnCount = turnCount;
            this.regen = regen;
        }

        /*public override void OnOverlapEffect()
        {
            Effect_005 oldEffect = Common.UnitAction.GetEffectByNumber(owner, number) as Effect_005;

            if (oldEffect.turnCount > turnCount)
                turnCount = oldEffect.turnCount;
            if (oldEffect.regen)

            if (oldEffect != null)
                owner.StateEffects.Remove(oldEffect);
        }*/

        public override void OnTurnStart()
        {
            Common.UnitAction.Heal(owner, regen);

            turnCount--;
            if (turnCount == 0)
                Common.UnitAction.RemoveEffect(owner, this);   
        }
    }
}