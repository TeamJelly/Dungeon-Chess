namespace Model.Artifacts
{
    /// <summary>
    /// 치명률 증가 중 + 2
    /// </summary>
    public class CRI_M : Artifact
    {
        protected int increasingValue = 2;
        public CRI_M()
        {
            Name = "CRI_M";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
        }

        public override void OnAdd()
        {
            Owner.CriRate += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.CriRate -= increasingValue;
        }
    }
}