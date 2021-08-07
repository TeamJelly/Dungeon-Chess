using Model.Effects;

namespace Model.Items
{
    class Barrior : Item
    {
        Barrior()
        {
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed_705");
            Color = UnityEngine.Color.yellow;
            Target = TargetType.Any;
        }

        public override void Use(Unit unit)
        {
            Common.Command.AddEffect(unit, new Barrier(unit,3));
        }
    }
}