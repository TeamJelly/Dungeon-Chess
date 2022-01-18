using Model.Effects;

namespace Model.Items
{
    class Fast : Item
    {
        public Fast()
        {
            Name = "Fast Potion";
            UserTarget = TargetType.Any;

            SpriteNumber = 705;
            InColor = UnityEngine.Color.cyan;
            OutColor = UnityEngine.Color.clear;
        }

        public override void Use(Tile tile)
        {
            Unit unit = tile.GetUnit();
            if (unit != null)
                Common.Command.AddEffect(unit, new Effects.Fast());
        }
    }
}