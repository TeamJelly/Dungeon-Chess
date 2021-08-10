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
            Name = "별의 목걸이";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Legend;
            Description = "행동력 +3";
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