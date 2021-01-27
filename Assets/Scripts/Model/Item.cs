using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [System.Serializable]
    public class Item
    {
        public int number;
        public string name = "No Skill Name";
        public UnitClass unitClass = UnitClass.NULL;
    }
}