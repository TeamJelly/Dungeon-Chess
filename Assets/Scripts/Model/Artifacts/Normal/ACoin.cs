namespace Model.Artifacts.Normal
{
    /// <summary>
    /// 치명률 증가 소 + 1
    /// </summary>
    public class ACoin : Artifact
    {
        protected int increasingValue = 100;
        public ACoin()
        {
            Name = "동전 한 닢";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Normal;
            Description = "골드 +100";
        }

        public override void OnAdd()
        {
            
        }
    }
}