using UnityEditor;
using UnityEngine;
using Model;
using Model.Skills;

namespace Model.Units
{
    public class Unit_000 : Unit
    {
        public Unit_000() : base(0)
        {
            MoveSkill = new Walk();
            Skills[0] = new Skill_001();
        }
    }
}