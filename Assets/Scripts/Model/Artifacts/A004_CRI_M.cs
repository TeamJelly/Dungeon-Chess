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
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
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