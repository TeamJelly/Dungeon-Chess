using UnityEngine;

namespace Model.Effects
{
    [System.Serializable]
    public class Extension_005 : Common.Extensionable
    {
        public int turnCount;
        public int regen;
    }

    public class Effect_005 : Effect
    {
        private Extension_005 extension_005;
        public int TurnCount 
        {
            get => extension_005.turnCount;
            set => extension_005.turnCount = value;
        }

        public Effect_005(Unit owner, int turnCount) : base(owner)
        {
            descriptor.number = 5;
            descriptor.name = "재생";
            descriptor.description = "부여된 턴 동안, 턴 시작시 회복 수치만큼 HP를 회복한다.";
            
            if (Extension != null)
            {
                extension_005 = Common.Extension.Parse<Extension_005>(Extension);
                TurnCount = turnCount;
            }
            else
            {
                // 디버깅용
                extension_005 = new Extension_005();
                TurnCount = turnCount;
                extension_005.regen = 10;
            }
        }

        public override void OnAddThisEffect()
        {
            OnOverlapEffect();
            Debug.Log($"{Owner.Name}에게 {Name}효과 {TurnCount}턴 동안 추가됨");
        }

        public override void OnTurnStart()
        {
            Common.UnitAction.Heal(Owner, extension_005.regen);
            TurnCount--;

            Debug.Log($"{Name}효과 {TurnCount}턴 남음");

            if (TurnCount == 0)
            {
                Common.UnitAction.RemoveEffect(Owner, this);
            }
        }
    }
}