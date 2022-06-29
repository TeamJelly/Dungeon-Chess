namespace Model.Artifacts.Legend
{
    public class FirstFire : Artifact
    {
        protected int increasingValue;
        public FirstFire()
        {
            Name = "First Fire";
            Grade = ArtifactGrade.Legend;
            Description = "매턴 몬스터 체력 1% 감소";

            SpriteNumber = 662;
            InColor = UnityEngine.Color.yellow;
            OutColor = UnityEngine.Color.clear;
        }
    }
}