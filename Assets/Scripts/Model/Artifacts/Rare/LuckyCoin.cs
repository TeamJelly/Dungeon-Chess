namespace Model.Artifacts.Rare
{
    public class LuckyCoin : Artifact
    {
        protected int increasingValue;
        public LuckyCoin()
        {
            Name = "행운의 동전";
            Sprite = Common.Data.Colored[184];
            Grade = ArtifactGrade.Rare;
            Description = "돈 획득량 +1%";
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