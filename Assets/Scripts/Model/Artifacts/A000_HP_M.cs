namespace Model.Artifacts
{
    /// <summary>
    /// HP 증가 중 + 10
    /// </summary>
    public class A000_HP_M : Artifact
    {
        protected int increasingValue = 10;
        public A000_HP_M()
        {
            Name = "a";
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
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