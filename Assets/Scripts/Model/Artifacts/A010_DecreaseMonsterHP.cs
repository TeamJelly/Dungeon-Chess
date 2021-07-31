namespace Model.Artifacts
{
    public class A010_DecreaseMonsterHP : Artifact
    {
        protected int increasingValue;
        public A010_DecreaseMonsterHP()
        {
            Name = "k";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
        }

        public override void OnAddThisEffect()
        {

        }

        public override void OnRemoveThisEffect()
        {

        }
    }
}