namespace Model.Artifacts
{
    /// <summary>
    /// HP 증가 중 + 10
    /// </summary>
    public class HP_M : Artifact
    {
        protected int increasingValue = 10;
        public HP_M()
        {
            Name = "HP_M";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
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