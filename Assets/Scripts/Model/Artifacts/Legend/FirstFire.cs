namespace Model.Artifacts.Legend
{
    public class FirstFire : Artifact
    {
        protected int increasingValue;
        public FirstFire()
        {
            Name = "DecreaseMonsterHP";
            Sprite = Common.Data.Colored[494];
            Grade = ArtifactGrade.Normal;
            Description = "매턴 몬스터 체력 1% 감소";
        }
    }
}