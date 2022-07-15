// using Model.Effects;

// namespace Model.Items
// {
//     class Poison : Item
//     {
//         // public Poison()
//         // {
//         //     Name = "Poison Potion";
//         //     UserTarget = TargetType.Any;

//         //     SpriteNumber = 705;
//         //     InColor = UnityEngine.Color.magenta;
//         //     OutColor = UnityEngine.Color.clear;
//         // }
//         public override void Use(Tile tile)
//         {
//             Unit unit = tile.GetUnit();
//             if (unit != null)
//                 Common.Command.AddEffect(unit, new Effects.Poison());
//         }
//     }
// }