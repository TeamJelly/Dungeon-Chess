namespace Model.Artifacts.Rare
{
    public class LuckyCoin : Artifact
    {
        protected int increasingValue;
        public LuckyCoin()
        {
            Name = "행운의 동전";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }
        bool ArtifactFunction(bool b)
        {
            //기능 구현
            return b;
        }
    }
}