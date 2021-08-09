namespace Model.Items
{
    class Damage : Item
    {
        Damage()
        {
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed_705");
            Color = UnityEngine.Color.red;
            Target = TargetType.Any;
        }
        public override void Use(Tile tile)
        {
            Unit unit = tile.GetUnit();
            if (unit != null)
                Common.Command.Damage(unit, 10);
        }
    }
}