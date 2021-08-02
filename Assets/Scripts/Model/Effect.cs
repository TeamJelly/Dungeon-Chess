using UnityEngine;

namespace Model
{
    public class Effect : Spriteable
    {
        public Unit Owner { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }

        private int turnCount;
        public int TurnCount
        {
            get => turnCount;
            set => turnCount = value;
        }

        public Sprite Sprite { get; set; }

        public Color Color => throw new System.NotImplementedException();

        public Effect()
        {

        }
        public Effect(Unit owner)
        {
            Owner = owner;
        }

        public Effect(Unit owner, int turnCount)
        {
            Owner = owner;
            TurnCount = turnCount;
        }

        public virtual void OnAddThisEffect()
        {
            Effect oldEffect = Common.Command.GetEffectByNumber(Owner, Number);

            if (oldEffect != null)
                OnOverlapEffect(oldEffect);

            Debug.Log($"{Owner.Name}에게 {Name} 효과 추가됨");
        }

        /// <summary>
        /// 효과의 중복 검사와 중복 처리를 해준다.
        /// </summary>
        public virtual void OnOverlapEffect(Effect oldEffect)
        {
            Owner.StateEffects.Remove(oldEffect);
        }

        public virtual void OnRemoveThisEffect()
        {

        }

        public virtual void OnBattleStart()
        {

        }

        public virtual void OnBattleEnd()
        {

        }

        public virtual bool OnTurnStart(bool value)
        {
            return value;
        }

        public virtual bool OnTurnEnd(bool value)
        {
            return value;
        }

        public virtual void BeforeMove()
        {

        }

        public virtual void AfterMove()
        {

        }

        public virtual void BeforeUseSkill(Skill skill)
        {

        }

        public virtual void AfterUseSkill()
        {

        }

        public virtual int BeforeGetDamage(int value)
        {
            return value;
        }

        public virtual int AfterGetDamamge(int value)
        {
            return value;
        }

        public virtual void OnGetOtherEffect()
        {

        }

        public virtual void BeforeUseItem()
        {

        }

        public virtual void AfterUseItem()
        {

        }

        public virtual void BeforeUnitDie()
        {

        }
    }
}