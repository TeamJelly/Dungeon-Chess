namespace Model.Artifacts.Normal
{
    /// <summary>
    /// 이동력 증가 소 + 3
    /// </summary>
    public class LeatherBoots : Artifact
    {
        protected int increasingValue = 1;
        public LeatherBoots()
        {
            Name = "가죽 장화";
            Sprite = Common.Data.Colored[39];
            Grade = ArtifactGrade.Normal;
            Description = "이동력 +1";
        }

        public override void OnAdd(Unit owner)
        {
            base.OnAdd(owner);
            Owner.Mobility += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Mobility -= increasingValue;
        }
    }
}