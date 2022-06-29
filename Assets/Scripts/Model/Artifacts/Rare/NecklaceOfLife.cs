namespace Model.Artifacts.Rare
{
    /// <summary>
    /// 행동력 증가 중 + 2
    /// </summary>
    public class NecklaceOfLife : Artifact
    {
        protected int increasingValue = 2;
        public NecklaceOfLife()
        {
            Name = "Necklace Of Life";
            Grade = ArtifactGrade.Rare;
            Description = "행동력 +2";

            SpriteNumber = 427;
            InColor = UnityEngine.Color.red;
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