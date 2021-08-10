namespace Model.Artifacts.Legend
{
    public class FirstFire : Artifact
    {
        protected int increasingValue;
        public FirstFire()
        {
            Name = "DecreaseMonsterHP";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Normal;
            Description = "매턴 몬스터 체력 1% 감소";
        }

        public override void OnAdd()
        {

        }

        public override void OnRemove()
        {

        }
    }
}