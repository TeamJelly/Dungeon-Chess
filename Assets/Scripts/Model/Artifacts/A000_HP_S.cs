namespace Model.Artifacts
{
    /// <summary>
    /// HP 증가 소 + 5
    /// </summary>
    public class A000_HP_S : Artifact
    {
        protected int increasingValue = 5;
        public A000_HP_S()
        {
            Name = "a";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
        }

        public override void OnAddThisEffect()
        {
            Owner.MaxHP += increasingValue;
        }

        public override void OnRemoveThisEffect()
        {
            Owner.MaxHP -= increasingValue;
        }
    }
}