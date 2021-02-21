using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Proto_Priest : Unit
    {
        public Proto_Priest() : base(2)
        {
            MoveSkill = new Walk();
        }
    }
}
