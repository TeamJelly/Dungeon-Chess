namespace Model.Artifacts
{
    /// <summary>
    /// 행동력 증가 소 + 1
    /// </summary>
    public class AGI_S : Artifact
    {
        protected int increasingValue = 1;
        public AGI_S()
        {
            Name = "AGI_S";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Common;
        }

        public override void OnAdd()
        {
            Owner.Agility += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.Agility -= increasingValue;
        }
    }
}