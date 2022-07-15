// namespace Model.Items
// {

//     //한글 깨짐 테스트....ㅁㄴㅇㄹ
//     class Heal : Item
//     {
//         // public Heal()
//         // {
//         //     Name = "Heal Potion";
//         //     UserTarget = TargetType.Any;

//         //     SpriteNumber = 705;
//         //     InColor = UnityEngine.Color.green;
//         //     OutColor = UnityEngine.Color.clear;
//         // }

//         public override void Use(Tile tile)
//         {
//             Unit unit = tile.GetUnit();
//             if (unit != null)
//                 Common.Command.Heal(unit,10);
//         }
//     }
// }