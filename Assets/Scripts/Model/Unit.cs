using UnityEngine;
using System.Collections.Generic;
using Common.DB;
using Common;

public enum Category { NULL, Party, Neutral, Friendly, Enemy};
public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };

namespace Model
{
    using Skills;
    using System;
    using Units;
    [System.Serializable]
    public class Unit
    {
        // NOTE 리펙토링 될 사항임
        // public bool IsModified = false;

        private UnitDescriptor descriptor = new UnitDescriptor();
        public UnitDescriptor UnitDescriptor => descriptor;

        private Vector2Int position;
        private float actionRate;
        private int moveCount;
        private int skillCount;
        private int itemCount;
        private Skill moveSkill;// = new Walk();
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

        public string Name => descriptor.name;

        public Category Category { get => descriptor.category; set => descriptor.category = value; }

        public UnitClass UnitClass => descriptor.unitClass;

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
                if (descriptor.armor != value)
                {
                    descriptor.armor = value;
                    OnAmor.changed.Invoke(value);
                    if (descriptor.armor < value)
                    {
                        OnAmor.up.Invoke(value);
                    } else
                    {
                        OnAmor.down.Invoke(value);
                    }
                    
                }
            }
        }
        public UpAndDownEventListener<int> OnAmor = new UpAndDownEventListener<int>();

        public int Level {
            get => descriptor.level;
            set
            {
                if (descriptor.level != value)
                {
                    descriptor.level = value;
                    OnLevel.changed.Invoke(value);
                    if (descriptor.level < value)
                    {
                        OnLevel.up.Invoke(value);
                    }
                    else
                    {
                        OnLevel.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnLevel = new UpAndDownEventListener<int>();

        private int currentHP;
        public int CurrentHP {
            get => currentHP;
            set
            {
                if (currentHP != value)
                {
                    currentHP = value;
                    OnCurrentHP.changed.Invoke(value);
                    if (currentHP < value)
                    {
                        OnCurrentHP.up.Invoke(value);
                    }
                    else
                    {
                        OnCurrentHP.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnCurrentHP = new UpAndDownEventListener<int>();

        public int MaximumHP {
            get => descriptor.HP;
            set
            {
                if (descriptor.HP != value)
                {
                    descriptor.HP = value;
                    OnMaximumHP.changed.Invoke(value);
                    if (descriptor.HP < value)
                    {
                        OnMaximumHP.up.Invoke(value);
                    }
                    else
                    {
                        OnMaximumHP.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnMaximumHP = new UpAndDownEventListener<int>();

        private int currentEXP;
        public int CurrentEXP {
            get => currentEXP;
            set
            {
                if (currentEXP != value)
                {
                    currentEXP = value;
                    OnCurrentEXP.changed.Invoke(value);
                    if (currentEXP < value)
                    {
                        OnCurrentEXP.up.Invoke(value);
                    }
                    else
                    {
                        OnCurrentEXP.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnCurrentEXP = new UpAndDownEventListener<int>();

        private int nextEXP;
        public int NextEXP {
            get => nextEXP;
            set
            {
                if (nextEXP != value)
                {
                    nextEXP = value;
                    OnNextEXP.changed.Invoke(value);
                    if (nextEXP < value)
                    {
                        OnNextEXP.up.Invoke(value);
                    }
                    else
                    {
                        OnNextEXP.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnNextEXP = new UpAndDownEventListener<int>();

        public int Strength {
            get => descriptor.strength;
            set
            {
                if (descriptor.strength != value)
                {
                    descriptor.strength = value;
                    OnStrength.changed.Invoke(value);
                    if (descriptor.strength < value)
                    {
                        OnStrength.up.Invoke(value);
                    }
                    else
                    {
                        OnStrength.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnStrength = new UpAndDownEventListener<int>();

        public int Agility {
            get => descriptor.agility;
            set
            {
                if (descriptor.agility != value)
                {
                    descriptor.agility = value;
                    OnAgility.changed.Invoke(value);
                    if (descriptor.agility < value)
                    {
                        OnAgility.up.Invoke(value);
                    }
                    else
                    {
                        OnAgility.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnAgility = new UpAndDownEventListener<int>();

        public int Move {
            get => descriptor.move;
            set
            {
                if (descriptor.move != value)
                {
                    descriptor.move = value;
                    OnMove.changed.Invoke(value);
                    if (descriptor.move < value)
                    {
                        OnMove.up.Invoke(value);
                    }
                    else
                    {
                        OnMove.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnMove = new UpAndDownEventListener<int>();

        public Vector2Int Position {
            get => position;
            set {
                if (Managers.BattleManager.instance != null && Managers.BattleManager.IsAvilablePosition(Position))
                {
                    if (position != value)
                    {
                        position = value;
                        OnPositionChanged.Invoke(value);
                    }
                    
                }
                else
                    Debug.LogError("유닛을 이곳으로 이동할 수 없습니다.");
            }
        }
        public EventListener<Vector2Int> OnPositionChanged = new EventListener<Vector2Int>();

        public float ActionRate {
            get => actionRate;
            set
            {
                if (actionRate != value)
                {
                    actionRate = value;
                    OnActionRate.changed.Invoke(value);
                    if (actionRate < value)
                    {
                        OnActionRate.up.Invoke(value);
                    }
                    else
                    {
                        OnActionRate.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<float> OnActionRate = new UpAndDownEventListener<float>();

        public int MoveCount {
            get => moveCount;
            set
            {
                if (moveCount != value)
                {
                    moveCount = value;
                    OnMoveCount.changed.Invoke(value);
                    if (moveCount < value)
                    {
                        OnMoveCount.up.Invoke(value);
                    }
                    else
                    {
                        OnMoveCount.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnMoveCount = new UpAndDownEventListener<int>();
        public int SkillCount {
            get => skillCount;
            set
            {
                if (skillCount != value)
                {
                    skillCount = value;
                    OnSkillCount.changed.Invoke(value);
                    if (skillCount < value)
                    {
                        OnSkillCount.up.Invoke(value);
                    }
                    else
                    {
                        OnSkillCount.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnSkillCount = new UpAndDownEventListener<int>();
        public int ItemCount {
            get => itemCount;
            set
            {
                if (itemCount != value)
                {
                    itemCount = value;
                    OnItemCount.changed.Invoke(value);
                    if (itemCount < value)
                    {
                        OnItemCount.up.Invoke(value);
                    }
                    else
                    {
                        OnItemCount.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnItemCount = new UpAndDownEventListener<int>();
        public Skill MoveSkill {
            get => moveSkill;
            set
            {
                moveSkill = value;
            }
        }
        public Skill[] Skills {
            get => skills;
            set
            {
                skills = value;
            }
        }
        public Skill[] Items {
            get => items;
            set
            {
                items = value;
            }
        }
        public List<Artifact> Antiques {
            get => antiques;
            set
            {
                antiques = value;
            }
        }
        public List<Effect> StateEffects {
            get => stateEffects;
            set
            {
                stateEffects = value;
            }
        }

        public int Critical { 
            get => descriptor.critical;
            set
            {
                if (descriptor.critical != value)
                {
                    descriptor.critical = value;
                    OnCritical.changed.Invoke(value);
                    if (descriptor.critical < value)
                    {
                        OnCritical.up.Invoke(value);
                    }
                    else
                    {
                        OnCritical.down.Invoke(value);
                    }

                }
            }
        }
        public UpAndDownEventListener<int> OnCritical = new UpAndDownEventListener<int>();
        /// <summary>
        /// DB로 부터 초기화
        /// </summary>
        /// <param name="id">DB 키값</param>
        protected Unit(int id)
        {
            initializeUnitFromDB(id);
            CurrentHP = MaximumHP;

            var skillString = descriptor.skills.Split(';');

            //descriptor로부터 걷기 추가
            moveSkill = (Skill)Activator.CreateInstance(Type.GetType($"Model.Skills.{descriptor.moveSkill}"));

            //descriptor로부터 스킬 추가
            for (int i = 0; i < skillString.Length; i++)
            {
                // Debug.Log(skillString[i]);
                skills[i] = (Skill)Activator.CreateInstance(Type.GetType($"Model.Skills.{skillString[i]}"));
            }
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