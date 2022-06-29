using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Managers;
using View;
using UnityEngine.Tilemaps;

namespace Model.Tiles
{
    public class Power : Tile
    {
        public Power()
        {
            TileBase = Resources.Load<TileBase>("1bitpack_kenney_1/Tilesheet/TileBases/Strength");
            category = TileCategory.Power;
            Initials = "PW";
        }

        public int strength = 3;

        public override void OnTile(Unit unit)
        {
            base.OnTile(unit);
            if (unit == null) return;
            AnimationManager.ReserveFadeTextClips(unit, $"STR +{strength}", Color.green);
            unit.Strength += strength;


            //자리를 벗어나는 즉시 호출되는 OffTile 대신
            //이동이 전부 끝난 후에 호출되는 일회용 콜백 등록.
            Vector2Int RemoveBuffCallback(Vector2Int v)
            {
                AnimationManager.ReserveFadeTextClips(unit, $"STR -{strength}", Color.green);
                unit.Strength -= strength;
                unit.OnMove.after.RemoveListener(RemoveBuffCallback);
                return v;
            }

            unit.OnMove.after.AddListener(RemoveBuffCallback);
        }
    }
}