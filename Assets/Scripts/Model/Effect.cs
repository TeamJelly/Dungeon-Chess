using UnityEngine;

namespace Model
{
    using Effects;
    public class Effect
    {
        protected EffectDescriptor descriptor = new EffectDescriptor();

        public Unit Owner;
        public string Name => descriptor.name;
        public int Number => descriptor.number;
        public string Extension => descriptor.extension;
        public string Description => descriptor.description;
        private int turnCount;
        public int TurnCount
        {
            get => turnCount;
            set => turnCount = value;
        }
        public Effect(Unit owner, int number)
        {
            Owner = owner;
            InitializeEffectFromDB(number);
        }
        private void InitializeEffectFromDB(int number)
        {
            var _descriptor = EffectStorage.Instance[number];
            if (_descriptor != null)
            {
                descriptor = _descriptor.Copy();
            }
            else
            {
                Debug.LogError($"number={number}에 해당하는 효과가 없습니다.");
            }
        }

        /// <summary>
        /// 효과의 중복 검사와 중복 처리를 해준다.
        /// </summary>
        public virtual void OnOverlapEffect(Effect oldEffect)
        {
            Owner.StateEffects.Remove(oldEffect);
        }

        public virtual void OnAddThisEffect()
        {
            Effect oldEffect = Common.UnitAction.GetEffectByNumber(Owner, Number);
            if(oldEffect != null)
                OnOverlapEffect(oldEffect);
            Debug.Log($"{Owner.Name}에게 {Name} 효과 추가됨");
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

        public virtual void OnTurnStart()
        {

        }

        public virtual void OnTurnEnd()
        {

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

        public virtual int BeforeGetDamage(int damage)
        {
            return damage;
        }

        public virtual int AfterGetDamamge(int damage)
        {
            return damage;
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