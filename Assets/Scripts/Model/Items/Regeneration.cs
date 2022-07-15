// using Model.Effects;

// namespace Model.Items
// {
//     class Regeneration : Item
//     {
//         // public Regeneration()
//         // {
//         //     Name = "Regeneration Potion";
//         //     UserTarget = TargetType.Any;

//         //     SpriteNumber = 705;
//         //     InColor = UnityEngine.Color.green;
//         //     OutColor = UnityEngine.Color.clear;
//         // }
//         public override void Use(Tile tile)
//         {
//             Unit unit = tile.GetUnit();
//             if (unit != null)
//                 Common.Command.AddEffect(unit, new Effects.Regeneration());
//         }
//     }
// }