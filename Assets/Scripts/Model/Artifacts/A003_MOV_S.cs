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
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
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