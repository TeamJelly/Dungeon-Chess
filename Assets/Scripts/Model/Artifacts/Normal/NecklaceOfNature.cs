namespace Model.Artifacts.Normal
{
    /// <summary>
    /// 행동력 증가 소 + 1
    /// </summary>
    public class NecklaceOfNature : Artifact
    {
        protected int increasingValue = 1;
        public NecklaceOfNature()
        {
            Name = "AGI_S";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Normal;
            Description = "행동력 +1";
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