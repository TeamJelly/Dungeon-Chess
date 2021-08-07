namespace Model.Artifacts
{
    /// <summary>
    /// HP 증가 대 + 15
    /// </summary>
    public class HP_L : Artifact
    {
        protected int increasingValue = 15;
        public HP_L()
        {
            Name = "HP_L";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31"); 
            Grade = ArtifactGrade.Legend;
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