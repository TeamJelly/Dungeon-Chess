namespace Model.Artifacts
{
    /// <summary>
    /// 이동력 증가 소 + 3
    /// </summary>
    public class MOV_S : Artifact
    {
        protected int increasingValue = 1;
        public MOV_S()
        {
            Name = "MOV_S";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Common;
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