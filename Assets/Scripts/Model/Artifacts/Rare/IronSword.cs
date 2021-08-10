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
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
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