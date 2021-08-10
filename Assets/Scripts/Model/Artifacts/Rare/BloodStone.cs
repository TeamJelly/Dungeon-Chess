using Common;
namespace Model.Artifacts.Rare
{
    public class BloodStone : Artifact
    {
        public BloodStone()
        {
            Name = "혈석";
            Sprite = Common.Data.Colored[513];
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