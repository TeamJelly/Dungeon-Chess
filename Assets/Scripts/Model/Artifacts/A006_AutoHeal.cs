using Common;
using UnityEngine;

namespace Model.Artifacts
{
    public class A006_AutoHeal : Artifact
    {
        public A006_AutoHeal()
        {
            Name = "g";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
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