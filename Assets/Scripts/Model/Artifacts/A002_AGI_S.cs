namespace Model.Artifacts
{
    /// <summary>
    /// 행동력 증가 소 + 1
    /// </summary>
    public class A002_AGI_S : Artifact
    {
        protected int increasingValue = 1;
        public A002_AGI_S()
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