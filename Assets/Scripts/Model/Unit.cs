using UnityEngine;
using System.Collections.Generic;
using Common.DB;

public enum Category { NULL, Party, Neutral, Friendly, Enemy};
public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };

namespace Model
{
    using Skills;
    using Units;
    [System.Serializable]
    public class Unit
    {
        public bool IsModified = false;

        private UnitDescriptor descriptor = new UnitDescriptor();
        public UnitDescriptor UnitDescriptor => descriptor;

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

        private int partyID; // 파티 내부에서 고유 아이디

        public enum AnimationState
        {
            Idle, Hit, Attack, Move, Heal
        }

        private Sprite portrait;
        private RuntimeAnimatorController animator;

        public AnimationState animationState = AnimationState.Idle;

        public int ID => descriptor.id;

        public string Name {
            get => descriptor.name;
            set
            {
                descriptor.name = value;
                IsModified = true;
            }
        }

        public Category Category {
            get => descriptor.category;
            set {
                descriptor.category = value;
                IsModified = true;
            }
        }

        public UnitClass UnitClass {
            get => descriptor.unitClass;
            set
            {
                descriptor.unitClass = value;
                IsModified = true;
            }
        }

        public Sprite Portrait
        {
            get
            {
                if (portrait == null && descriptor.portraitPath != "")
                    portrait = Resources.Load<Sprite>(descriptor.portraitPath);
                return portrait;
            }
        }

        public RuntimeAnimatorController Animator
        {
            get
            {
                if (animator == null && descriptor.animatorPath != "")
                    animator = Resources.Load<RuntimeAnimatorController>(descriptor.animatorPath);
                return animator;
            }
        }

        public int Armor
        {
            get => descriptor.armor;
            set
            {
                Debug.Log($"{Name}가(은) 방어도가 변했다! [Armor: {Armor} > {value}");
                descriptor.armor = value;
                IsModified = true;
            }
        }

        public int Level {
            get => descriptor.level;
            set
            {
                descriptor.level = value;
                IsModified = true;                
            }
        }

        private int currentHP;
        public int CurrentHP {
            get => currentHP;
            set
            {                
                currentHP = value;
                IsModified = true;
            }
        }

        public int MaximumHP {
            get => descriptor.HP;
            set
            {
                descriptor.HP = value;
                IsModified = true;
            }
        }

        private int currentEXP;
        public int CurrentEXP {
            get => currentEXP;
            set
            {
                currentEXP = value;
                IsModified = true;
            }
        }

        private int nextEXP;
        public int NextEXP {
            get => nextEXP;
            set
            {
                nextEXP = value;
                IsModified = true;
            }
        }

        public int Strength {
            get => descriptor.strength;
            set
            {
                descriptor.strength = value;
                IsModified = true;
            }
        }

        public int Agility {
            get => descriptor.agility;
            set
            {
                descriptor.agility = value;
                IsModified = true;
            }
        }

        public int Move {
            get => descriptor.move;
            set
            {
                descriptor.move = value;
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
            get => descriptor.critical; 
            set
            {
                descriptor.critical = value;
                IsModified = true;
            }
        }
        /// <summary>
        /// DB로 부터 초기화
        /// </summary>
        /// <param name="id">DB 키값</param>
        protected Unit(int id)
        {
            initializeUnitFromDB(id);
            CurrentHP = MaximumHP;
        }
        private void initializeUnitFromDB(int no)
        {
            var _descriptor = UnitStorage.Instance[no];
            if (_descriptor != null)
            {
                descriptor = _descriptor.Copy();
            }
            else
            {
                Debug.LogError($"number={no}에 해당하는 유닛이 없습니다.");
            }
        }
    }
}