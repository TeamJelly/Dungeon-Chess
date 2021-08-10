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
            Name = "자연의 목걸이";
            Sprite = Common.Data.Colored[330];
            Grade = ArtifactGrade.Normal;
            Description = "행동력 +1";
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