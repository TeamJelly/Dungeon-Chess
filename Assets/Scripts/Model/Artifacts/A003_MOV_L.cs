namespace Model.Artifacts
{
    /// <summary>
    /// 이동력 증가 대 + 3
    /// </summary>
    public class A003_MOV_L : Artifact
    {
        protected int increasingValue = 3;
        public A003_MOV_L()
        {
            Name = "d";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
        }

        public override void OnAddThisEffect()
        {
            Owner.Mobility += increasingValue;
        }

        public override void OnRemoveThisEffect()
        {
            Owner.Mobility -= increasingValue;
        }
    }
}