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