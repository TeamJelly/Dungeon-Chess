namespace Model.Artifacts
{
    /// <summary>
    /// 행동력 증가 중 + 2
    /// </summary>
    public class A002_AGI_M : Artifact
    {
        protected int increasingValue = 2;
        public A002_AGI_M()
        {
            Name = "b";
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
        }

        public override void OnAddThisEffect()
        {
            Owner.Agility += increasingValue;
        }

        public override void OnRemoveThisEffect()
        {
            Owner.Agility -= increasingValue;
        }
    }
}