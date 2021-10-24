using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using View;

namespace Model.Effects
{
    public class Bind : Effect
    {
        public Bind(Unit owner) : base(owner)
        {
            Name = "속박";
            Description = "유닛이 한 턴동안 움직이지 못합니다.";
        }

        // 이미 기절 효과가 발동 됨을 기록한다.
        bool isActivated = false;

        public override void OnAdd()
        {
            base.OnAdd();

            Owner.IsMoved = true;

            Owner.OnTurnStart.before.AddListener(OnTurnStart);
            Owner.OnTurnEnd.after.AddListener(OnTurnEnd);
        }

        public override void OnRemove()
        {
            Owner.OnTurnStart.before.RemoveListener(OnTurnStart);
            Owner.OnTurnEnd.after.RemoveListener(OnTurnEnd);
        }

        public override bool OnTurnStart(bool value)
        {
            FadeOutTextView.MakeText(Owner, $"+Bind", Color.red);

            Owner.IsMoved = true;
            isActivated = true;

            return value;
        }

        public override bool OnTurnEnd(bool value)
        {
            if (isActivated)
                Common.Command.RemoveEffect(Owner, this);

            return value;
        }
    }
}
public class Restraint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
