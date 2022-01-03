namespace Model.Artifacts.Legend
{
    /// <summary>
    /// HP 증가 대 + 15
    /// </summary>
    public class LegenaryApple : Artifact
    {
        protected int increasingValue = 15;
        public LegenaryApple()
        {
            Name = "Legenary Apple";
            Grade = ArtifactGrade.Legend;
            Description = "최대체력 +15";

            SpriteNumber = 896;
            InColor = UnityEngine.Color.yellow;
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