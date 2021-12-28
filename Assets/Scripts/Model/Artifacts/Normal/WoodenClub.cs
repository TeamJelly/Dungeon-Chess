namespace Model.Artifacts.Normal
{
    /// <summary>
    /// 공격력 증가 소 + 1
    /// </summary>
    public class WoodenClub : Artifact
    {
        protected int increasingValue = 1;
        public WoodenClub()
        {
            Name = "나무 몽둥이";
            Sprite = Common.Data.Colored[127];
            Grade = ArtifactGrade.Normal;
            Description = "공격력 +1";
        }

        public override void OnAdd(Unit owner)
        {
            base.OnAdd(owner);
            Owner.Strength += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Strength -= increasingValue;
        }
    }
}