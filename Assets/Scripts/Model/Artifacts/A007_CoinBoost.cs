namespace Model.Artifacts
{
    public class A007_CoinBoost : Artifact
    {
        protected int increasingValue;
        public A007_CoinBoost()
        {
            Name = "e";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
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