namespace Model.Artifacts.Rare
{
    /// <summary>
    /// 치명률 증가 중 + 2
    /// </summary>
    public class SilverRing : Artifact
    {
        protected int increasingValue = 2;
        public SilverRing()
        {
            Name = "Silver Ring";
            Grade = ArtifactGrade.Rare;
            Description = "치명률 +10%";

            SpriteNumber = 331;
            InColor = UnityEngine.Color.gray;
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