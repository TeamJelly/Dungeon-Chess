namespace Model.Artifacts.Legend
{
    /// <summary>
    /// 치명률 증가 대 + 3
    /// </summary>
    public class GoldRing : Artifact
    {
        protected int increasingValue = 3;
        public GoldRing()
        {
            Name = "Golden Ring";
            Grade = ArtifactGrade.Legend;            
            Description = "치명률 +15%";

            SpriteNumber = 332;
            InColor = UnityEngine.Color.yellow;
            OutColor = UnityEngine.Color.clear;
        }

        public override void OnAdd()
        {
            Owner.CriRate += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.CriRate -= increasingValue;
        }
    }
}