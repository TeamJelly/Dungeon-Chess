namespace Model.Artifacts.Rare
{/// <summary>
 /// 공격력 증가 중 + 2
 /// </summary>
    public class IronSword : Artifact
    {
        protected int increasingValue = 3;
        public IronSword()
        {
            Name = "강철 대검";
            Grade = ArtifactGrade.Legend;
            Description = "공격력 +2";

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