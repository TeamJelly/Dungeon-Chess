namespace Model.Artifacts
{
    public class DecreaseMonsterHP : Artifact
    {
        protected int increasingValue;
        public DecreaseMonsterHP()
        {
            Name = "DecreaseMonsterHP";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Common;
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }
    }
}