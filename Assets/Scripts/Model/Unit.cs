using Common;
using UnityEngine;

public enum Category { NULL, Party, Neutral, Friendly, Enemy, Boss};
public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };
public enum UnitAI {NULL, AI1, AI2, AI3, AI4 };

namespace Model
{
    public class Unit
    {
        [Header("Normal Status")]
        public string name = "NoName";

        public Category category = Category.NULL;
        public UnitClass unitClass = UnitClass.NULL;
        public UnitAI unitAI = UnitAI.NULL;

        [Range(1, 99)]
        public int level;

        [Min(0)]
        public int currentHP, maxHP;

        [Min(0)]
        public int currentEXP, nextEXP;

        [Header("Basic Status")]
        public int strength;

        [Header("Special Status")]
        public int agility;
        public int move;
        public int critical;

        [Header("Position")]
        public Vector2Int position;

        [Header("Hidden Status")]
        public float actionRate;

        [Header("Count Status")]
        public int moveCount;
        public int skillCount;
        public int itemCount;

    }
}

