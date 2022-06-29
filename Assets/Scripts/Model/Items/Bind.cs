using Model.Effects;

namespace Model.Items
{
    class Bind : Item
    {
        public Bind()
        {
            Name = "Bind Scroll";
            UserTarget = TargetType.Any;

            SpriteNumber = 705;
            InColor = UnityEngine.Color.gray;
            OutColor = UnityEngine.Color.clear;
        }

        public override void Use(Tile tile)
        {
            Unit unit = tile.GetUnit();
            if (unit != null)
                Common.Command.AddEffect(unit, new Effects.Bind());
        }
    }
}