namespace Model.Artifacts
{
    public class CoinBoost : Artifact
    {
        protected int increasingValue;
        public CoinBoost()
        {
            Name = "CoinBoost";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }
        bool ArtifactFunction(bool b)
        {
            //기능 구현
            return b;
        }
    }
}