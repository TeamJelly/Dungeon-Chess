namespace Model.Artifacts.Rare
{
    /// <summary>
    /// 치명률 증가 중 + 2
    /// </summary>
    public class SilverRing : Artifact
    {
        protected int increasingValue = 2;
        public SilverRing()
        {
            Name = "은 반지";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
            Description = "치명률 +10%";
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