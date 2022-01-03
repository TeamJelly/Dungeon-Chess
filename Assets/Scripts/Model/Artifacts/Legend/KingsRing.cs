namespace Model.Artifacts.Legend
{
    public class KingsRing : Artifact
    {
        protected int increasingValue;
        public KingsRing()
        {
            Name = "King's Ring";
            Grade = ArtifactGrade.Legend;
            Description = "상점 물품을 50% 가격으로 구매한다.";

            SpriteNumber = 333;
            InColor = UnityEngine.Color.yellow;
            OutColor = UnityEngine.Color.clear;
        }
    }
}