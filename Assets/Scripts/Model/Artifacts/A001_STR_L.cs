namespace Model.Artifacts
{/// <summary>
 /// 공격력 증가 대 + 3
 /// </summary>
    public class A001_STR_L : Artifact
    {
        protected int increasingValue = 3;
        public A001_STR_L()
        {
            Name = "b";
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
        }

        public override void OnAddThisEffect()
        {
            Owner.Strength += increasingValue;
        }

        public override void OnRemoveThisEffect()
        {
            Owner.Strength -= increasingValue;
        }
    }
}