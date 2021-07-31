namespace Model.Artifacts
{
    /// <summary>
    /// 치명률 증가 대 + 3
    /// </summary>
    public class CRI_L : Artifact
    {
        protected int increasingValue = 3;
        public CRI_L()
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