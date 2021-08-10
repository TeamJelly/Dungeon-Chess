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
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
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