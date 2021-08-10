namespace Model.Artifacts.Legend
{
    /// <summary>
    /// 이동력 증가 대 + 3
    /// </summary>
    public class HerosBoots : Artifact
    {
        protected int increasingValue = 3;
        public HerosBoots()
        {
            Name = "영웅의 장화";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");           
            Grade = ArtifactGrade.Legend;
            Description = "이동력 +3";
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