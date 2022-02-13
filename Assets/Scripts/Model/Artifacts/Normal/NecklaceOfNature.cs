namespace Model.Artifacts.Normal
{
    /// <summary>
    /// 행동력 증가 소 + 1
    /// </summary>
    public class NecklaceOfNature : Artifact
    {
        protected int increasingValue = 1;
        public NecklaceOfNature()
        {
            Name = "Necklace Of Nature";
            Grade = ArtifactGrade.Normal;
            Description = "행동력 +1";

            SpriteNumber = 330;
            InColor = UnityEngine.Color.green;
            OutColor = UnityEngine.Color.clear;
        }

        public override void OnAdd()
        {
            Owner.Agility += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Agility -= increasingValue;
        }
    }
}