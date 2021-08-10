namespace Model.Artifacts.Rare
{
    public class AngelsWing : Artifact
    {
        public AngelsWing()
        {
            Name = "천사의 날개";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Legend;
            Description = "타일의 효과를 무시하고 움직인다.";
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }
    }
}