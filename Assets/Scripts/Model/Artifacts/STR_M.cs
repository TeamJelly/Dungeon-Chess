namespace Model.Artifacts
{
    /// <summary>
    /// 공격력 증가 중 + 2
    /// </summary>

    public class STR_M : Artifact
    {
        protected int increasingValue = 2;
        public STR_M()
        {
            Name = "STR_M";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
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