namespace Model.Items
{

    //한글 깨짐 테스트....ㅁㄴㅇㄹ
    class Heal : Item
    {
        public Heal()
        {
            Name = "힐 포션";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed_705");
            Color = UnityEngine.Color.green;
            Target = TargetType.Any;
        }

        public override void Use(Tile tile)
        {
            Unit unit = tile.GetUnit();
            if (unit != null)
                Common.Command.Heal(unit,10);
        }
    }
}