using UnityEngine;
using System.Collections;

namespace Model.Effects
{
    public class Effect_021 : Effect
    {
        public int barrierCount;

        public Effect_021(Unit owner, int count): base(owner)
        {
            descriptor.number = 21;
            descriptor.name = "보호막";
            descriptor.description = "보호막의 수 만큼 공격의 데미지를 0로 만든다.";
            barrierCount = count;
        }

        public override int BeforeGetDamage(int damage)
        {
            damage = 0;
            barrierCount--;
            if (barrierCount == 0) Common.UnitAction.RemoveEffect(Owner, this);
            return base.BeforeGetDamage(damage);
        }
    }
}