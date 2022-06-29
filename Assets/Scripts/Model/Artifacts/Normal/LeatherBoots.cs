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
            Name = "Leather Boots";
            Grade = ArtifactGrade.Normal;
            Description = "이동력 +1";

            SpriteNumber = 39;
            InColor = UnityEngine.Color.gray;
            OutColor = UnityEngine.Color.clear;
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