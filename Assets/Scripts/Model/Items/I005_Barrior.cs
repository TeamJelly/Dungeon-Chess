using Model.Effects;

namespace Model.Items
{
    class I005_Barrior : Item
    {
        I005_Barrior()
        {
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed_705");
            Color = UnityEngine.Color.green;
            Target = TargetType.Any;
        }

        public override void Use(Unit unit)
        {
            Common.Command.AddEffect(unit, new Barrier(unit,3));
        }
    }
}