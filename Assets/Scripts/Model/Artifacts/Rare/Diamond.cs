namespace Model.Artifacts.Rare
{
    public class Diamond : Artifact
    {
        protected int increasingValue;
        public Diamond()
        {
            Name = "다이아몬드";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
            Description = "골드 +200";
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