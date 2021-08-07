namespace Model.Artifacts
{
    public class Discount50 : Artifact
    {
        protected int increasingValue;
        public Discount50()
        {
            Name = "Discount50";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }
    }
}