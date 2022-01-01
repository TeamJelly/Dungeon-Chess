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
            Name = "금 반지";
            Sprite = Common.Data.Colored[332];
            Grade = ArtifactGrade.Legend;
            Description = "치명률 +15%";
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