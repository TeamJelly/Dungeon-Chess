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
            Name = "생명의 목걸이";
            Sprite = Common.Data.Colored[427];
            Grade = ArtifactGrade.Rare;
            Description = "행동력 +2";
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