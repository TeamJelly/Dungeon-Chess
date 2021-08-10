namespace Model.Artifacts.Legend
{
    /// <summary>
    /// 공격력 증가 대 + 3
    /// </summary>

    public class HerosSword : Artifact
    {
        protected int increasingValue = 2;
        public HerosSword()
        {
            Name = "영웅의 검";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
            Description = "공격력 +3";
        }

        public override void OnAdd()
        {
            Owner.Strength += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Strength -= increasingValue;
        }
    }
}