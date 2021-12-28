namespace Model.Artifacts.Rare
{
    /// <summary>
    /// HP 증가 중 + 10
    /// </summary>
    public class FreshMeat : Artifact
    {
        protected int increasingValue = 10;
        public FreshMeat()
        {
            Name = "신선한 고기";
            Sprite = Common.Data.Colored[800];
            Grade = ArtifactGrade.Rare;
            Description = "최대체력 +10";
        }

        public override void OnAdd(Unit owner)
        {
            base.OnAdd(owner);
            Owner.MaxHP += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.MaxHP -= increasingValue;
        }
    }
}