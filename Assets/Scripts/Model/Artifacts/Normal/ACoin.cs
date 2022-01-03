namespace Model.Artifacts.Normal
{
    /// <summary>
    /// 치명률 증가 소 + 1
    /// </summary>
    public class ACoin : Artifact
    {
        protected int increasingValue = 100;
        public ACoin()
        {
            Name = "A Coin";
            Grade = ArtifactGrade.Normal;
            Description = "골드 +100";

            SpriteNumber = 213;
            InColor = UnityEngine.Color.yellow;
            OutColor = UnityEngine.Color.clear;
        }
    }
}