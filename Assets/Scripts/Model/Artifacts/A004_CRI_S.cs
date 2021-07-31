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
            Name = "e";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
        }

        public override void OnAddThisEffect()
        {
            Owner.CriticalRate += increasingValue;
        }

        public override void OnRemoveThisEffect()
        {
            Owner.CriticalRate -= increasingValue;
        }
    }
}