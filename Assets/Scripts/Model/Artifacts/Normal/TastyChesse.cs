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
            Name = "맛좋은 치즈";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_801");
            Grade = ArtifactGrade.Normal;
            Description = "최대체력 +5";
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