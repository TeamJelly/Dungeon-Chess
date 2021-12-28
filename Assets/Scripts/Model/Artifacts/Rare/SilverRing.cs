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
            Name = "은 반지";
            Sprite = Common.Data.Colored[331];
            Grade = ArtifactGrade.Rare;
            Description = "치명률 +10%";
        }

        public override void OnAdd(Unit owner)
        {
            base.OnAdd(owner);
            Owner.CriRate += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.CriRate -= increasingValue;
        }
    }
}