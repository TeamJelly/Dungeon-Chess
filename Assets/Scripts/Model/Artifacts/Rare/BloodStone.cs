using Common;
using UnityEngine;
namespace Model.Artifacts.Rare
{
    public class BloodStone : Artifact
    {
        public BloodStone()
        {
            Name = "Blood Stone";
            Sprite = Common.Data.Colored[513];
            Grade = ArtifactGrade.Rare;
            Description = "턴 종료시 체력을 1 회복한다.";
        }

        public override void OnAdd()
        {
            Owner.OnTurnEnd.after.AddListener(ArtifactFunction);
        }

        public override void OnRemove()
        {
            Owner.OnTurnEnd.after.RemoveListener(ArtifactFunction);
        }

        bool ArtifactFunction(bool b)
        {
            Command.Heal(Owner, 1);
            return b;
        }
    }
}