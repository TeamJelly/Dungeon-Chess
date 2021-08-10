namespace Model.Artifacts.Legend
{
    public class KingsRing : Artifact
    {
        protected int increasingValue;
        public KingsRing()
        {
            Name = "왕의 반지";
            Sprite = Common.Data.Colored[333];
            Grade = ArtifactGrade.Rare;
            Description = "상점 물품을 50% 가격으로 구매한다.";
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }
    }
}