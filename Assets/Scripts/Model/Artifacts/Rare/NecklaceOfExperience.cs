namespace Model.Artifacts.Rare
{
    public class NecklaceOfExperience : Artifact
    {
        public NecklaceOfExperience()
        {
            Name = "경험의 목걸이";
            Sprite = Common.Data.Colored[428];
            Grade = ArtifactGrade.Normal;
            Description = "경험치 획득량 +10%";
        }
        bool ArtifactFunction(bool b)
        {
            //기능 구현
            return b;
        }
    }
}