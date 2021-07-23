namespace Model.Artifacts
{
    /// <summary>
    /// 공격력 증가 소 + 1
    /// </summary>
    public class A001_STR_S : Artifact
    {
        protected int increasingValue = 1;
        public A001_STR_S()
        {
            Name = "b";
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
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