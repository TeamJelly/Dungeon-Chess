namespace Model.Artifacts
{
    /// <summary>
    /// 이동력 증가 대 + 3
    /// </summary>
    public class MOV_L : Artifact
    {
        protected int increasingValue = 3;
        public MOV_L()
        {
            Name = "MOV_L";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");           
            Grade = ArtifactGrade.Legend;
        }

        public override void OnAdd()
        {
            Owner.Mobility += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Mobility -= increasingValue;
        }
    }
}