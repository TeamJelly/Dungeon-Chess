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
        public bool IsModified = false;

        private string name = "NoName";
        private Category category = Category.NULL;
        private UnitClass unitClass = UnitClass.NULL;
        private UnitAI unitAI = UnitAI.NULL;
        public string spritePath;
        private Sprite sprite;
        private int level;
        private int armor;
        private int currentHP;
        private int maximumHP;
        private int currentEXP;
        private int nextEXP;
        private int strength;
        private int agility;
        private int move = 3;
        private int critical;
        private Vector2Int position;
        private float actionRate;
        private int moveCount;
        private int skillCount;
        private int itemCount;

        private Skill moveSkill = new Walk();
        private Skill[] skills = new Skill[4];
        private Skill[] items = new Skill[2];
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

        public string Name {
            get => name;
            set
            {
                name = value;
                IsModified = true;
            }
        }
        public Category Category {
            get => category;
            set {
                category = value;
                IsModified = true;
            }
        }
        public UnitClass UnitClass {
            get => unitClass;
            set
            {
                unitClass = value;
                IsModified = true;
            }
        }
        public UnitAI UnitAI {
            get => unitAI;
            set
            {
                unitAI = value;
                IsModified = true;
            }
        }
        public Sprite Sprite
        {
            get
            {
                if (sprite == null && spritePath != "")
                    sprite = Resources.Load<Sprite>(spritePath);
                return sprite;
            }
        }

        public int Armor
        {
            get => armor;
            set
            {
                Debug.Log($"{Name}가(은) 방어도가 변했다! [Armor: {Armor} > {value}");
                armor = value;
                IsModified = true;
            }
        }
        public int Level {
            get => level;
            set
            {
                level = value;
                IsModified = true;
            }
        }
        public int CurrentHP {
            get => currentHP;
            set
            {
                currentHP = value;
                IsModified = true;
            }
        }
        public int MaximumHP {
            get => maximumHP;
            set
            {
                maximumHP = value;
                IsModified = true;
            }
        }
        public int CurrentEXP {
            get => currentEXP;
            set
            {
                currentEXP = value;
                IsModified = true;
            }
        }
        public int NextEXP {
            get => nextEXP;
            set
            {
                nextEXP = value;
                IsModified = true;
            }
        }
        public int Strength {
            get => strength;
            set
            {
                strength = value;
                IsModified = true;
            }
        }
        public int Agility {
            get => agility;
            set
            {
                agility = value;
                IsModified = true;
            }
        }
        public int Move {
            get => move;
            set
            {
                move = value;
                IsModified = true;
            }
        }
        public Vector2Int Position {
            get => position;
            set {
                if (Managers.BattleManager.instance != null && Managers.BattleManager.IsAvilablePosition(Position))
                {
                    position = value;
                    IsModified = true;
                }
                else
                    Debug.LogError("유닛을 이곳으로 이동할 수 없습니다.");
            }
        }
        public float ActionRate {
            get => actionRate;
            set
            {
                actionRate = value;
                IsModified = true;
            }
        }
        public int MoveCount {
            get => moveCount;
            set
            {
                moveCount = value;
                IsModified = true;
            }
        }
        public int SkillCount {
            get => skillCount;
            set
            {
                skillCount = value;
                IsModified = true;
            }
        }
        public int ItemCount {
            get => itemCount;
            set
            {
                itemCount = value;
                IsModified = true;
            }
        }
        public Skill MoveSkill {
            get => moveSkill;
            set
            {
                moveSkill = value;
                IsModified = true;
            }
        }
        public Skill[] Skills {
            get => skills;
            set
            {
                skills = value;
                IsModified = true;
            }
        }
        public Skill[] Items {
            get => items;
            set
            {
                items = value;
                IsModified = true;
            }
        }
        public List<Artifact> Antiques {
            get => antiques;
            set
            {
                antiques = value;
                IsModified = true;
            }
        }
        public List<Effect> StateEffects {
            get => stateEffects;
            set
            {
                stateEffects = value;
                IsModified = true;
            }
        }

        public int Critical { 
            get => critical; 
            set
            {
                critical = value;
                IsModified = true;
            }
        }

        public Unit() { }

        public Unit(string name)
        {
            this.name = name;
        }
    }
}