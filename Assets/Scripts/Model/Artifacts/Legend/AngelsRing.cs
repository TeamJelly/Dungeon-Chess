namespace Model.Artifacts.Legend
{
    public class AngelsRing : Artifact
    {
        public AngelsRing()
        {
            Name = "천사의 링";
            Sprite = Common.Data.Colored[662];
            Grade = ArtifactGrade.Legend;
            Description = "타일의 효과를 무시하고 움직인다.";
        }
    }
}