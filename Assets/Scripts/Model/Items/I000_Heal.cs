namespace Model.Items
{
    /*        public enum TargetType // 스킬의 대상
        {
            NULL = -1,
            Any,            // 모든 타일에 사용가능
            NoUnit,         // 유닛이 없는 곳에만 사용가능, 이동 혹은 소환류 스킬에 사용
            Friendly,       // 우호적인 유닛에 사용가능 (AI 용)
            Hostile,        // 적대적인 유닛에 사용가능 (AI 용)
        }
     */
    class I000_Heal : Item
    {
        public I000_Heal()
        {
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/monochrome_transparent_packed_705");
            Color = UnityEngine.Color.green;
            Target = TargetType.Any;
        }

        public override void Use(Unit unit)
        {
            Common.Command.Heal(unit,10);
        }
    }
}