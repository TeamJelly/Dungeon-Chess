namespace Model.Artifacts.Legend
{
    /// <summary>
    /// 행동력 증가 대 + 3
    /// </summary>
    public class Starlight : Artifact
    {
        protected int increasingValue = 3;
        public Starlight()
        {
            Name = "Starlight";
            Grade = ArtifactGrade.Legend;
            Description = "행동력 +3";

            SpriteNumber = 429;
            InColor = UnityEngine.Color.cyan;
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