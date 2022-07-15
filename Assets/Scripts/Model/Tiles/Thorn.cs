// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Model;
// using Model.Managers;
// using View;
// using UnityEngine.Tilemaps;

// namespace Model.Tiles
// {
//     public class Thorn : Tile
//     {
//         // public Thorn()
//         // {
//         //     TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/thorn");
//         //     category = TileCategory.Thorn;
//         //     Initials = "TN";
//         // }

//         public int damage = 3;

//         public override void OnTile(Unit unit)
//         {
//             base.OnTile(unit);
//             Common.Command.Damage(unit, 3);
//         }
//     }
// }