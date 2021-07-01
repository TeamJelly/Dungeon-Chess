using UnityEngine;
using System.Collections.Generic;
using Common.DB;
using Common;

namespace Model
{
    using Skills;
    using System;
    using Units;

    [System.Serializable]
    public class Unit
    {
        public enum UnitAlliance { NULL, Party, Neutral, Friendly, Enemy };
        public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };
        public enum AnimationState { Idle, Hit, Attack, Move, Heal };

        public string Name { get; set; }            // 이름
        private int level;
        private int curEXP;                         // 현재 경험치
        private int nextEXP;                        // 다음 레벨업에 필요한 경험치 
        private int curHP;                          // 현재 체력
        private int maxHP;                          // 최대 체력
        private int armor;                          // 방어도 (추가 체력)
        private int strength;                       // 힘
        private int agility;                        // 민첩
        private int move;                           // 이동력
        private int moveCount;                      // 이동가능 횟수
        private int skillCount;                     // 스킬가능 횟수
        private float actionRate;                   // 행동가능 퍼센테이지
        private Vector2Int position;                // 위치
        private Skill moveSkill;                    // 이동 스킬
        private Skill passiveSkill;                 // 패시브 스킬
        private Skill[] skills = new Skill[4];      // 4가지 스킬
        private List<Artifact> antiques = new();    // 보유한 유물
        private List<Effect> stateEffects = new();  // 보유한 상태효과

        private RuntimeAnimatorController animator; // 애니메이터

        public AnimationState animationState = AnimationState.Idle; // 현재 애니메이션 상태
        public UnitAlliance Alliance { get; set; }  // 진영
        public UnitClass Class { get; set; }        // 직업

        private readonly string portraitPath = "";
        private Sprite portrait;
        public Sprite Portrait
        {
            get
            {
                if (portrait == null)
                    portrait = Common.Data.LoadSprite(portraitPath);
                return portrait;
            }
        }

        // 아머 up 혹은 아머 break 효과 추가 필요
        public int Armor
        {
            get => armor;
            set
            {
                if (armor != value)
                {
                    OnAmor.changed.Invoke(value);
                    if (armor < value)
                    {
                        OnAmor.up.Invoke(value);
                    }
                    else
                    {
                        OnAmor.down.Invoke(value);
                    }
                    armor = value;
                }
            }
        }
        public UpAndDownEventListener<int> OnAmor = new UpAndDownEventListener<int>();

        // 레벨업 효과 추가 필요
        public int Level {
            get => level;
            set
            {
                if (level != value)
                {
                    OnLevel.changed.Invoke(value);
                    if (level < value)
                    {
                        OnLevel.up.Invoke(value);
                    }
                    else
                    {
                        OnLevel.down.Invoke(value);
                    }
                    level = value;
                }
            }
        }
        public UpAndDownEventListener<int> OnLevel = new UpAndDownEventListener<int>();

        // HP가 변했을때 효과 추가 필요
        public int CurHP {
            get => curHP;
            set
            {
                if (curHP != value)
                {
                    BeforeChangeCurHP.changed.Invoke(value);
                    if (curHP < value)
                    {
                        BeforeChangeCurHP.up.Invoke(value);
                        curHP = value;
                        AfterChangeCurHP.up.Invoke(value);
                    }
                    else
                    {
                        BeforeChangeCurHP.down.Invoke(value);
                        curHP = value;
                        AfterChangeCurHP.down.Invoke(value);
                    }
                    AfterChangeCurHP.changed.Invoke(value);
                }
            }
        }
        public UpAndDownEventListener<int> BeforeChangeCurHP = new();
        public UpAndDownEventListener<int> AfterChangeCurHP = new();

        // 최대 HP는 딱히 효과가 있을지 모르겠다.
        public int MaxHP {
            get => maxHP;
            set
            {
                if (maxHP != value)
                {
                    OnMaxHP.changed.Invoke(value);
                    if (maxHP < value)
                    {
                        OnMaxHP.up.Invoke(value);
                    }
                    else
                    {
                        OnMaxHP.down.Invoke(value);
                    }
                    maxHP = value;
                }
            }
        }
        public UpAndDownEventListener<int> OnMaxHP = new UpAndDownEventListener<int>();

        // EXP가 상승했을때 효과 추가 필요
        public int CurEXP {
            get => curEXP;
            set
            {
                if (curEXP != value)
                {
                    OnCurEXP.changed.Invoke(value);
                    if (curEXP < value)
                    {
                        OnCurEXP.up.Invoke(value);
                    }
                    else
                    {
                        OnCurEXP.down.Invoke(value);
                    }
                    curEXP = value;
                }
            }
        }
        public UpAndDownEventListener<int> OnCurEXP = new UpAndDownEventListener<int>();

        // 효과 추가 필요없음
        public int NextEXP {
            get => nextEXP;
            set
            {
                if (nextEXP != value)
                {
                    OnNextEXP.changed.Invoke(value);
                    if (nextEXP < value)
                    {
                        OnNextEXP.up.Invoke(value);
                    }
                    else
                    {
                        OnNextEXP.down.Invoke(value);
                    }
                    nextEXP = value;
                }
            }
        }
        public UpAndDownEventListener<int> OnNextEXP = new UpAndDownEventListener<int>();

        // 효과 추가 필요없음
        public int Strength {
            get => strength;
            set
            {
                if (strength != value)
                {
                    OnStrength.changed.Invoke(value);
                    if (strength < value)
                    {
                        OnStrength.up.Invoke(value);
                    }
                    else
                    {
                        OnStrength.down.Invoke(value);
                    }
                    strength = value;
                }
            }
        }
        public UpAndDownEventListener<int> OnStrength = new UpAndDownEventListener<int>();

        public int Agility {
            get => agility;
            set
            {
                if (agility != value)
                {
                    OnAgility.changed.Invoke(value);
                    if (agility < value)
                    {
                        OnAgility.up.Invoke(value);
                    }
                    else
                    {
                        OnAgility.down.Invoke(value);
                    }
                    agility = value;
                }
            }
        }
        public UpAndDownEventListener<int> OnAgility = new UpAndDownEventListener<int>();

        public int Move {
            get => move;
            set
            {
                if (move != value)
                {
                    OnMove.changed.Invoke(value);
                    if (move < value)
                    {
                        OnMove.up.Invoke(value);
                    }
                    else
                    {
                        OnMove.down.Invoke(value);
                    }
                    move = value;
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
                        OnPositionChanged.Invoke(value);
                        position = value;
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
                    OnActionRate.changed.Invoke(value);
                    if (actionRate < value)
                    {
                        OnActionRate.up.Invoke(value);
                    }
                    else
                    {
                        OnActionRate.down.Invoke(value);
                    }
                    actionRate = value;
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
                    OnMoveCount.changed.Invoke(value);
                    if (moveCount < value)
                    {
                        OnMoveCount.up.Invoke(value);
                    }
                    else
                    {
                        OnMoveCount.down.Invoke(value);
                    }
                    moveCount = value;
                }
            }
        }
        public UpAndDownEventListener<int> OnMoveCount = new UpAndDownEventListener<int>();

        public int SkillCount {
            get => skillCount;
            set
            {
                // 스킬 가능 횟수가 변경되면
                if (skillCount != value)
                {
                    // 값이 변경됨 이벤트를 발생시킨다.
                    OnSkillCount.changed.Invoke(value);
                    if (skillCount < value)
                    {
                        // 값이 오름 이벤트를 발생시킨다.
                        OnSkillCount.up.Invoke(value);
                    }
                    else
                    {
                        // 값이 낮아짐 이벤트를 발생시킨다.
                        OnSkillCount.down.Invoke(value);
                    }
                    // 값을 변경해주고
                    skillCount = value;
                }
            }
        }
        public UpAndDownEventListener<int> OnSkillCount = new UpAndDownEventListener<int>();

        public Skill MoveSkill {
            get => moveSkill;
            set
            {
                moveSkill = value;
            }
        }

        public Skill PassiveSkill { 
            get => passiveSkill;
            set
            {
                passiveSkill = value;
            }
        }

        public Skill[] Skills {
            get => skills;
            set
            {
                skills = value;
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