namespace Model.Artifacts
{
    /// <summary>
    /// 공격력 증가 중 + 2
    /// </summary>

    public class A001_STR_M : Artifact
    {
        protected int increasingValue = 2;
        public A001_STR_M()
        {
            Name = "b";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
        }

        public override void OnAddThisEffect()
        {
            Owner.Strength += increasingValue;
        }

        public override void OnRemoveThisEffect()
        {
            Owner.Strength -= increasingValue;
        }
    }
}