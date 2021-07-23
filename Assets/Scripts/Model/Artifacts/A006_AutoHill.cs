using Common;
using UnityEngine;

namespace Model.Artifacts
{
    public class A006_AutoHill : Artifact
    {
        public A006_AutoHill()
        {
            Name = "g";
            spritePath = "1bitpack_kenney_1/Artifacts/A001_Helmet";
        }

        public override void OnAddThisEffect()
        {
            Owner.OnTurnStart.after.AddListener(ArtifactFunction);
        }

        public override void OnRemoveThisEffect()
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