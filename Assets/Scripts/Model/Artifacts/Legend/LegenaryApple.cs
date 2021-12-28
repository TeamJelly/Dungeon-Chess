namespace Model.Artifacts.Legend
{
    /// <summary>
    /// HP 증가 대 + 15
    /// </summary>
    public class LegenaryApple : Artifact
    {
        protected int increasingValue = 15;
        public LegenaryApple()
        {
            Name = "전설의 사과";
            Sprite = Common.Data.Colored[896];
            Grade = ArtifactGrade.Legend;
            Description = "최대체력 +15";
        }

        public override void OnAdd(Unit owner)
        {
            base.OnAdd(owner);
            Owner.MaxHP += increasingValue;
        }

        public override void OnRemove()
        {
            Owner.MaxHP -= increasingValue;
        }
    }
}