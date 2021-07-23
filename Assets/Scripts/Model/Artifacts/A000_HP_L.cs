namespace Model.Artifacts
{
    /// <summary>
    /// HP 증가 대 + 15
    /// </summary>
    public class A000_HP_L : Artifact
    {
        protected int increasingValue = 15;
        public A000_HP_L()
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