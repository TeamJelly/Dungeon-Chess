using UnityEngine;
using System.Collections;

namespace Model.Effects
{
    public class Effect_021 : Effect
    {
        public int barrierCount;

        public Effect_021(Unit owner, int count)
        {
            number = 21;
            name = "보호막";
            description = "보호막의 수 만큼 공격의 데미지를 0로 만든다.";
            barrierCount = count;
            this.owner = owner;
        }

        public override int BeforeGetDamage(int damage)
        {
            damage = 0;
            barrierCount--;
            if (barrierCount == 0) Common.UnitAction.RemoveEffect(owner, this);
            return base.BeforeGetDamage(damage);
        }
    }
}