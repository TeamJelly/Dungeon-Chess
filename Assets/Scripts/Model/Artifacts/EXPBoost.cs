namespace Model.Artifacts
{

    public class EXPBoost : Artifact
    {
        public EXPBoost()
        {
            Name = "EXPBoost";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Common;
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