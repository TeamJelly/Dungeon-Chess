using Common;
namespace Model.Artifacts.Rare
{
    public class BloodStone : Artifact
    {
        public BloodStone()
        {
            Name = "혈석";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
            Description = "매턴 종료시 체력회복 +1";
        }

        public override void OnAdd()
        {
            Owner.OnTurnStart.after.AddListener(ArtifactFunction);
        }

        public override void OnRemove()
        {
            Owner.OnTurnStart.after.RemoveListener(ArtifactFunction);
        }

        bool ArtifactFunction(bool b)
        {
            Command.Heal(Owner, 1);
            return b;
        }
    }
}