namespace Model.Artifacts.Legend
{
    /// <summary>
    /// 이동력 증가 대 + 3
    /// </summary>
    public class HerosBoots : Artifact
    {
        protected int increasingValue = 3;
        public HerosBoots()
        {
            Name = "Heros Boots";
            Sprite = Common.Data.Colored[87];
            Grade = ArtifactGrade.Legend;
            Description = "이동력 +3";
        }

        public override void OnAdd()
        {
            Owner.Mobility += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Mobility -= increasingValue;
        }
    }
}