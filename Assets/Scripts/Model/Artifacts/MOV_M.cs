namespace Model.Artifacts
{
    /// <summary>
    /// 이동력 증가 중 + 3
    /// </summary>
    public class MOV_M : Artifact
    {
        protected int increasingValue = 2;
        public MOV_M()
        {
            Name = "MOV_M";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
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