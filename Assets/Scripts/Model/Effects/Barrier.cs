using UnityEngine;
using System.Collections;
using View;

namespace Model.Effects
{

    public class Barrier : Effect
    {
        private int barrierCount;

        public int BarrierCount
        {
            get => barrierCount;
            set => barrierCount = value;
        }

        /// <summary>
        /// 배리어
        /// </summary>
        /// <param name="owner">효과 소유자</param>
        /// <param name="barrierCount">배리어의 개수</param>
        /// <param name="turnCount">배리어의 지속 턴 수</param>
        public Barrier()
        {
            Name = "Barrier";
            Description = $"보호막의 수({barrierCount}) 만큼 공격의 데미지를 무효화 한다.";
            BarrierCount = barrierCount;

            SpriteNumber = 182;
            InColor = Color.yellow;
            OutColor = Color.clear;
        }

        public override void OnAdd()
        {
            barrierCount = 1;
            Owner.OnDamage.before.AddListener(BeforeGetDam);
        }

        public override void OnRemove()
        {
            base.OnRemove();

            Owner.OnDamage.before.RemoveListener(BeforeGetDam);
        }

        public override int BeforeGetDam(int value)
        {
            AnimationManager.ReserveFadeTextClips(Owner, $"+Barrier ({--barrierCount})", Color.yellow);
            // 데미지 무효화
            value = 0;

            if (BarrierCount == 0)
                Common.Command.RemoveEffect(Owner, this);

            return value;
        }

        //public override bool OnTurnStart(bool value)
        //{
        //    if (--TurnCount == 0) 
        //        Common.UnitAction.RemoveEffect(Owner, this);

        //    View.FadeOutTextUI.MakeText(Owner.Position + Vector2Int.up, $"배리어 -1Turn", Color.red);

        //    return value;
        //}
    }
}