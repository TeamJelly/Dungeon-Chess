namespace Model.Artifacts
{
    /// <summary>
    /// 치명률 증가 소 + 1
    /// </summary>
    public class CRI_S : Artifact
    {
        protected int increasingValue = 1;
        public CRI_S()
        {
            Name = "CRI_S";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Common;
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