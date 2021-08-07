namespace Model.Artifacts
{
    public class IgnoreTile : Artifact
    {
        public IgnoreTile()
        {
            Name = "IgnoreTile";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Legend;
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }
    }
}