namespace Model.Artifacts
{
    /// <summary>
    /// 행동력 증가 중 + 2
    /// </summary>
    public class AGI_M : Artifact
    {
        protected int increasingValue = 2;
        public AGI_M()
        {
            Name = "AGI_M";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
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