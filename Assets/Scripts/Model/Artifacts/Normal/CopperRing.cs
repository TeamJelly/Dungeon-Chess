namespace Model.Artifacts.Normal
{
    /// <summary>
    /// 치명률 증가 소 + 1
    /// </summary>
    public class CopperRing : Artifact
    {
        protected int increasingValue = 1;
        public CopperRing()
        {
            Name = "구리 반지";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Normal;
            Description = "치명률 +5%";
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