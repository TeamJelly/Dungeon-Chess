using System;
using UnityEngine;

namespace Model
{
    public class Effect : Infoable
    {
        public Unit Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private int turnCount;

        public int TurnCount
        {
            get => turnCount;
            set => turnCount = value;
        }

        public virtual string Type => "Effect";

        public Sprite Sprite
        {
            get
            {
                if (sprite == null)
                    sprite = Common.Data.MakeSprite(SpriteNumber, InColor, OutColor);
                return sprite;
            }
            // set => sprite = value;
        }

        private Sprite sprite;
        public int SpriteNumber { get; set; }
        public Color InColor { get; set; }
        public Color OutColor { get; set; }

        //public Effect()
        //{
        //}
        //public Effect(Unit owner)
        //{
        //    Owner = owner;
        //}
        //public Effect(Unit owner, int turnCount)
        //{
        //    Owner = owner;
        //    TurnCount = turnCount;
        //}

        public virtual void OnAdd()
        {
            Debug.Log($"{Owner.Name}에게 {Name} 효과 추가됨");
        }

        /// <summary>
        /// 효과의 중복 검사와 중복 처리를 해준다.
        /// </summary>
        public virtual void OnOverlap()
        {
            Debug.Log($"{Owner.Name}에게 {Name} 효과 중복됨");
        }

        public virtual void OnRemove()
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

        public virtual int BeforeGetDam(int value)
        {
            return value;
        }

        public virtual int AfterGetDam(int value)
        {
            return value;
        }

        public object Clone()
        {
            Type t = GetType();

            return Activator.CreateInstance(t);
        }
    }
}