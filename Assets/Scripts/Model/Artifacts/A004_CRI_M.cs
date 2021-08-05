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
            Name = "e";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
        }

        public override void OnAddThisEffect()
        {
            Owner.CriRate += increasingValue;
        }

        public override void OnRemoveThisEffect()
        {
            Owner.CriRate -= increasingValue;
        }
    }
}