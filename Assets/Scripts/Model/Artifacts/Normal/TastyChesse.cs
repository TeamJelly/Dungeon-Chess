namespace Model.Artifacts.Normal
{
    /// <summary>
    /// HP 증가 소 + 5
    /// </summary>
    public class TastyChesse : Artifact
    {
        protected int increasingValue = 5;
        public TastyChesse()
        {
            Name = "Tasty Chesse";
            Grade = ArtifactGrade.Normal;
            Description = "최대체력 +5";

            SpriteNumber = 801;
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