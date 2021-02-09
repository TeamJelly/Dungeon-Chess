using Common;
using UnityEngine;
using System.Collections.Generic;

public enum Category { NULL, Party, Neutral, Friendly, Enemy, Boss};
public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };
public enum UnitAI {NULL, AI1, AI2, AI3, AI4 };

namespace Model
{
    using Skills;
    [System.Serializable]
    public class Unit
    {
        private string name = "NoName";
        private Category category = Category.NULL;
        private UnitClass unitClass = UnitClass.NULL;
        private UnitAI unitAI = UnitAI.NULL;
        public string spritePath;
        private Sprite sprite;
        private int level;
        private int currentHP;
        private int maximumHP;
        private int currentEXP;
        private int nextEXP;
        private int strength;
        private int agility;
        private int move = 3;
        private Vector2Int position;
        private float actionRate;
        private int moveCount;
        private int skillCount;
        private int itemCount;

        private Skill moveSkill = new Walk();
        private Skill[] skills = new Skill[4];
        private Item[] items = new Item[2];
        private List<Artifact> antiques = new List<Artifact>();
        private List<Effect> stateEffects = new List<Effect>();

        public List<Animation> animations;
        public Animation animation;
        public string animationPath1;
        public string animationPath2;
        public string animationPath3;
        public string animationPath4;

        public enum AnimationState
        {
            Idle, Hit, Attack, Move
        }

        public AnimationState animationState = AnimationState.Idle;

        public string Name { get => name; set => name = value; }
        public Category Category { get => category; set => category = value; }
        public UnitClass UnitClass { get => unitClass; set => unitClass = value; }
        public UnitAI UnitAI { get => unitAI; set => unitAI = value; }
        public Sprite Sprite
        {
            get
            {
                if (sprite == null && spritePath != "")
                    sprite = Resources.Load<Sprite>(spritePath);
                return sprite;
            }
        }
        public int Level { get => level; set => level = value; }
        public int CurrentHP { get => currentHP; set => currentHP = value; }
        public int MaximumHP { get => maximumHP; set => maximumHP = value; }
        public int CurrentEXP { get => currentEXP; set => currentEXP = value; }
        public int NextEXP { get => nextEXP; set => nextEXP = value; }
        public int Strength { get => strength; set => strength = value; }
        public int Agility { get => agility; set => agility = value; }
        public int Move { get => move; set => move = value; }
        public Vector2Int Position { get => position; 
            set{
                if (Managers.BattleManager.IsAvilablePosition(Position))
                    position = value;
                else
                    Debug.LogError("유닛을 이곳으로 이동할 수 없습니다.");
            } 
        }
        public float ActionRate { get => actionRate; set => actionRate = value; }
        public int MoveCount { get => moveCount; set => moveCount = value; }
        public int SkillCount { get => skillCount; set => skillCount = value; }
        public int ItemCount { get => itemCount; set => itemCount = value; }
        public Skill MoveSkill { get => moveSkill; set => moveSkill = value; }
        public Skill[] Skills { get => skills; set => skills = value; }
        public Item[] Items { get => items; set => items = value; }
        public List<Artifact> Antiques { get => antiques; set => antiques = value; }
        public List<Effect> StateEffects { get => stateEffects; set => stateEffects = value; }

        public Unit()
        {
        }

        public Unit(string name)
        {
            this.name = name;
        }
    }
}

