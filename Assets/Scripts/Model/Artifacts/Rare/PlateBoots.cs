namespace Model.Artifacts.Rare
{
    /// <summary>
    /// 이동력 증가 중 + 3
    /// </summary>
    public class PlateBoots : Artifact
    {
        protected int increasingValue = 2;
        public PlateBoots()
        {
            Name = "판금 장화";
            Sprite = Common.Data.Colored[39];
            Grade = ArtifactGrade.Rare;
            Description = "이동력 +2";
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