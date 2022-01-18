using Model.Effects;

namespace Model.Items
{
    class Barrior : Item
    {
        public Barrior()
        {
            Name = "Barrior Scroll";
            UserTarget = TargetType.Any;

            SpriteNumber = 705;
            InColor = UnityEngine.Color.yellow;
            OutColor = UnityEngine.Color.clear;
        }

        public override void Use(Tile tile)
        {
            Unit unit = tile.GetUnit();
            if (unit != null)
                Common.Command.AddEffect(unit, new Barrier());
        }
    }
}