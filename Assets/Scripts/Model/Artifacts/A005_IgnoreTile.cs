namespace Model.Artifacts
{
    public class A005_IgnoreTile : Artifact
    {
        public A005_IgnoreTile()
        {
            Name = "e";
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