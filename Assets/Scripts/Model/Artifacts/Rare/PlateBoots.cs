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
            Name = "Plate Boots";
            Grade = ArtifactGrade.Rare;
            Description = "이동력 +2";

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