namespace Model.Artifacts
{
    /// <summary>
    /// 공격력 증가 소 + 1
    /// </summary>
    public class STR_S : Artifact
    {
        protected int increasingValue = 1;
        public STR_S()
        {
            Name = "STR_S";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Common;
        }

        public override void OnAdd()
        {
            Owner.Strength += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Strength -= increasingValue;
        }
    }
}