namespace Model.Artifacts
{
    /// <summary>
    /// 행동력 증가 대 + 3
    /// </summary>
    public class AGI_L : Artifact
    {
        protected int increasingValue = 3;
        public AGI_L()
        {
            Name = "AGI_L";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Legend;
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