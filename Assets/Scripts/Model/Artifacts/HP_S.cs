namespace Model.Artifacts
{
    /// <summary>
    /// HP 증가 소 + 5
    /// </summary>
    public class HP_S : Artifact
    {
        protected int increasingValue = 5;
        public HP_S()
        {
            Name = "HP_S";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Common;
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