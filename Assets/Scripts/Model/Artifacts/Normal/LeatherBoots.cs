namespace Model.Artifacts.Normal
{
    /// <summary>
    /// 이동력 증가 소 + 3
    /// </summary>
    public class LeatherBoots : Artifact
    {
        protected int increasingValue = 1;
        public LeatherBoots()
        {
            Name = "가죽 장화";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Normal;
            Description = "이동력 +1";
        }

        public override void OnAdd()
        {
            Owner.Mobility += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Mobility -= increasingValue;
        }
    }
}