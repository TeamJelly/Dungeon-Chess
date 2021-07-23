namespace Model.Artifacts
{
    /// <summary>
    /// 이동력 증가 소 + 3
    /// </summary>
    public class A003_MOV_S : Artifact
    {
        protected int increasingValue = 1;
        public A003_MOV_S()
        {
            Name = "d";
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
        }

        public override void OnAddThisEffect()
        {
            Owner.Move += increasingValue;
        }

        public override void OnRemoveThisEffect()
        {
            Owner.Move -= increasingValue;
        }
    }
}