namespace Model.Items
{
    class Damage : Item
    {
        public Damage()
        {
            Name = "Damage Potion";
            UserTarget = TargetType.Any;

            SpriteNumber = 705;
            InColor = UnityEngine.Color.red;
            OutColor = UnityEngine.Color.clear;
        }
        public override void Use(Tile tile)
        {
            Unit unit = tile.GetUnit();
            if (unit != null)
                Common.Command.Damage(unit, 10);
        }
    }
}