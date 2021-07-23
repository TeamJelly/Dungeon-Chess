namespace Model.Artifacts
{
    public class A007_CoinBoost : Artifact
    {
        protected int increasingValue;
        public A007_CoinBoost()
        {
            Name = "e";
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
        }

        public override void OnAddThisEffect()
        {

        }

        public override void OnRemoveThisEffect()
        {

        }
        bool ArtifactFunction(bool b)
        {
            //기능 구현
            return b;
        }
    }
}