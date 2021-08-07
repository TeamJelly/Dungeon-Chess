using Model.Effects;

namespace Model.Items
{
    class Poison : Item
    {
        Poison()
        {
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed_705");
            Color = UnityEngine.Color.magenta;
            Target = TargetType.Any;
        }
        public override void Use(Unit unit)
        {
            Common.Command.AddEffect(unit, new Effects.Poison(unit,3));
        }
    }
}