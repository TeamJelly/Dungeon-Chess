using Model.Effects;

namespace Model.Items
{
    class Fast : Item
    {
        Fast()
        {
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed_705");
            Color = UnityEngine.Color.cyan;
            Target = TargetType.Any;
        }

        public override void Use(Unit unit)
        {
            Common.Command.AddEffect(unit, new Effects.Fast(unit,3));
        }
    }
}