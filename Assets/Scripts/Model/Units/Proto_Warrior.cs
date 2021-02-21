using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Proto_Warrior : Unit
    {
        public Proto_Warrior() : base(3)
        {
            MoveSkill = new Walk();
        }
    }
}