// using UnityEngine;
// using View;

// namespace Model.Effects
// {
//     public class Regeneration : StateEffect
//     {
//         private readonly int regen = 10;

//         public int Regen { get => regen; }

//         public Regeneration()
//         {
//             Name = "Regeneration";
//             Description = $"턴을 시작할때 체력 10을 회복합니다. 남은 턴 : {TurnCount}";

//             SpriteNumber = 563;
//             InColor = Color.green;
//             OutColor = Color.clear;
//         }

//         public override void OnAdd()
//         {
//             TurnCount = 3;

//             Owner.OnTurnStart.before.AddListener(OnTurnStart);
//             Owner.OnTurnEnd.after.AddListener(OnTurnEnd);
//             Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
//         }
//         public override void OnRemove()
//         {
//             base.OnRemove();
//             Owner.OnTurnStart.before.RemoveListener(OnTurnStart);
//             Owner.OnTurnEnd.after.RemoveListener(OnTurnEnd);
//         }

//         public override bool OnTurnStart(bool value)
//         {
//             AnimationManager.ReserveFadeTextClips(Owner, $"+Regen ({TurnCount})", Color.green);
//             Common.Command.Heal(Owner, Regen);

//             return value;
//         }
//         public override bool OnTurnEnd(bool value)
//         {
//             if (--TurnCount == 0)
//                 Common.Command.RemoveEffect(Owner, this);
//             return value;
//         }
//     }
// }