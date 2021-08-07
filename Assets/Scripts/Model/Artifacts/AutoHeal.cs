using Common;
using UnityEngine;

namespace Model.Artifacts
{
    public class AutoHeal : Artifact
    {
        public AutoHeal()
        {
            Name = "AutoHeal";
            Sprite = Common.Data.LoadSprite("1bitpack_kenney_1/Tilesheet/colored_transparent_packed_31");
            Grade = ArtifactGrade.Rare;
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