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
            Name = "Fresh Meat";
            Grade = ArtifactGrade.Rare;
            Description = "최대체력 +10";

            SpriteNumber = 800;
            InColor = UnityEngine.Color.red;
            OutColor = UnityEngine.Color.clear;
        }

        public override void OnAdd()
        {
            Owner.MaxHP += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.MaxHP -= increasingValue;
        }
    }
}