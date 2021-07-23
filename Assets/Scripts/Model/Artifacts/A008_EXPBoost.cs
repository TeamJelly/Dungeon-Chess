namespace Model.Artifacts
{

    public class A008_EXPBoost : Artifact
    {
        public A008_EXPBoost()
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