namespace Model.Artifacts.Rare
{
    public class Diamond : Artifact
    {
        protected int increasingValue;
        public Diamond()
        {
            Name = "다이아몬드";
            Sprite = Common.Data.Colored[214];
            Grade = ArtifactGrade.Rare;
            Description = "골드 +200";
        }
        bool ArtifactFunction(bool b)
        {
            //기능 구현
            return b;
        }
    }
}