namespace Model.Artifacts.Rare
{
    public class LuckyCoin : Artifact
    {
        protected int increasingValue;
        public LuckyCoin()
        {
            Name = "Lucky Coin";
            Grade = ArtifactGrade.Rare;
            Description = "돈 획득량 +1%";

            SpriteNumber = 184;
            InColor = UnityEngine.Color.yellow;
            OutColor = UnityEngine.Color.clear;
        }
        bool ArtifactFunction(bool b)
        {
            //기능 구현
            return b;
        }
    }
}