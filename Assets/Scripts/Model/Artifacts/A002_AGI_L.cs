namespace Model.Artifacts
{
    /// <summary>
    /// 행동력 증가 대 + 3
    /// </summary>
    public class A002_AGI_L : Artifact
    {
        protected int increasingValue = 3;
        public A002_AGI_L()
        {
            Name = "b";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
        }

        public override void OnAddThisEffect()
        {
            Owner.Agility += increasingValue;
        }

        public override void OnRemoveThisEffect()
        {
            Owner.Agility -= increasingValue;
        }
    }
}