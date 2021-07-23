namespace Model.Artifacts
{
    /// <summary>
    /// 행동력 증가 소 + 1
    /// </summary>
    public class A002_AGI_S : Artifact
    {
        protected int increasingValue = 1;
        public A002_AGI_S()
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