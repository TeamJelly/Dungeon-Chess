using UnityEngine;

namespace Model.Effects
{
    [System.Serializable]
    public class Extension_005 : Common.Extensionable
    {
        public int regen;
    }

    public class Effect_005 : Effect
    {
        private Extension_005 ext;

        public Effect_005(Unit owner, int turnCount) : base(owner, 5)
        {
            if(Extension.Length > 0)
            {
                ext = Common.Extension.Parse<Extension_005>(Extension);
            }
            TurnCount = turnCount;
        }

        public override void OnAddThisEffect()
        {
            base.OnAddThisEffect();
            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
        }

        public override void OnTurnStart()
        {
            Common.UnitAction.Heal(Owner, ext.regen);
            TurnCount--;

            Debug.Log($"{Name}효과 {TurnCount}턴 남음");

            if (TurnCount == 0)
            {
                Common.UnitAction.RemoveEffect(Owner, this);
            }
        }
    }
}