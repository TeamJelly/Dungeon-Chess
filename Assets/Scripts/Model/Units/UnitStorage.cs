using System.Collections;
using UnityEngine;
using Common.DB;
using System;

namespace Model.Units
{
    public class UnitStorage : Storage<int, UnitDescriptor>
    {
        private static UnitStorage instance = new UnitStorage();
        public static UnitStorage Instance => instance;
        private UnitStorage() : base("unit_table", "id") { }
    }
    [System.Serializable]
    public class UnitDescriptor: Copyable<UnitDescriptor>
    {
        public int id;
        public string name = "";
        public Alliance category;
        public UnitClass unitClass;
        public string portraitPath = "";
        public string animatorPath = "";
        public int level;
        public int HP;
        public int EXP;
        public int armor;
        public int strength;
        public int agility;
        public int move;
        public int critical;
        public string moveSkill = "";
        public string skills = "";
        public string antiques = "";
        public string items = "";
        public string stateEffects = "";
    }
}