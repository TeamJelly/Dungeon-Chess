using UnityEngine;
using System.Collections.Generic;
using Common;
using System;

namespace Model
{
    public enum UnitAlliance { NULL, Party, Neutral, Friendly, Enemy };
    public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };

    [Serializable]
    public class Unit
    {
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
        private int criticalRate;                 // 치명타 율
        private float actionRate;                   // 행동가능 퍼센테이지
        private Vector2Int position;                // 위치
        private Skill moveSkill;                    // 이동 스킬
        private Skill passiveSkill;                 // 패시브 스킬
        private Skill[] skills = new Skill[4];      // 4가지 스킬

        // 배울수 있는 스킬
        private Skill[] learnableSkill = new Skill[8];

        private List<Artifact> antiques = new List<Artifact>();    // 보유한 유물
        private List<Effect> stateEffects = new List<Effect>();  // 보유한 상태효과

        private RuntimeAnimatorController animator; // 애니메이터

        protected String animatorPath = "";
        public AnimationState animationState = AnimationState.Idle; // 현재 애니메이션 상태
        public UnitAlliance Alliance { get; set; }  // 진영
        public UnitClass Class { get; set; }        // 직업

        protected string portraitPath = "";
        private Sprite portrait;
        public Sprite Portrait
        {
            get
            {
                if (portrait == null)
                    portrait = Data.LoadSprite(portraitPath);
                return portrait;
            }
        }

        // 유닛 이벤트 모음
        public TimeEventListener<bool> OnBattleStart = new TimeEventListener<bool>();
        public TimeEventListener<bool> OnBattleEnd = new TimeEventListener<bool>();
        public TimeEventListener<bool> OnTurnStart = new TimeEventListener<bool>();
        public TimeEventListener<bool> OnTurnEnd = new TimeEventListener<bool>();

        public TimeEventListener<int> OnCurHP = new TimeEventListener<int>();
        public TimeEventListener<int> OnHeal = new TimeEventListener<int>();
        public TimeEventListener<int> OnDamage = new TimeEventListener<int>();
        public TimeEventListener<int> OnArmor = new TimeEventListener<int>();
        public TimeEventListener<int> OnLevel = new TimeEventListener<int>();
        public TimeEventListener<int> OnCurEXP = new TimeEventListener<int>();
        public TimeEventListener<Vector2Int> OnPosition = new TimeEventListener<Vector2Int>();

        // HP가 변했을때 효과 추가 필요
        public int CurHP
        {
            get => curHP;
            set
            {
                if (curHP != value)
                {
                    OnCurHP.before.RefInvoke(ref value);
                    curHP = value;
                    OnCurHP.after.RefInvoke(ref value);
                }
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
                    OnArmor.before.RefInvoke(ref value);
                    armor = value;
                    OnArmor.after.RefInvoke(ref value);
                }
            }
        }

        // 레벨업 효과 추가 필요
        public int Level {
            get => level;
            set
            {
                if (level != value)
                {
                    OnLevel.before.RefInvoke(ref value);
                    level = value;
                    OnLevel.after.RefInvoke(ref value);
                }
            }
        }

        public int CurEXP
        {
            get => curEXP;
            set
            {
                if (curEXP != value)
                {
                    OnCurEXP.before.RefInvoke(ref value);
                    curEXP = value;
                    OnCurEXP.after.RefInvoke(ref value);
                }
            }
        }

        public Vector2Int Position
        {
            get => position;
            set
            {
                if (Managers.BattleManager.instance != null && Managers.FieldManager.IsInField(Position))
                {
                    if (position != value)
                    {
                        OnPosition.before.RefInvoke(ref value);
                        OnPosition.before.Invoke(value);
                        position = value;
                        OnPosition.after.RefInvoke(ref value);
                        OnPosition.after.Invoke(value);
                    }
                }
                else
                    Debug.LogError("유닛을 이곳으로 이동할 수 없습니다.");
            }
        }

        public int NextEXP { get => nextEXP; set => nextEXP = value; }
        public int Strength { get => strength; set => strength = value; }
        public int MaxHP { get => maxHP; set => maxHP = value; }
        public int Agility { get => agility; set => agility = value; }
        public int Move { get => move; set => move = value; }
        public float ActionRate { get => actionRate; set => actionRate = value; }
        public int SkillCount { get => skillCount; set => skillCount = value; }
        public Skill MoveSkill { get => moveSkill; set => moveSkill = value; }
        public Skill PassiveSkill { get => passiveSkill; set => passiveSkill = value; }
        public Skill[] Skills { get => skills; set => skills = value; }
        public List<Artifact> Antiques { get => antiques; set => antiques = value; }
        public List<Effect> StateEffects { get => stateEffects; set => stateEffects = value; }
        public int MoveCount { get => moveCount; set => moveCount = value; }
        public int CriticalRate { get => criticalRate; set => criticalRate = value; }
        public RuntimeAnimatorController Animator
        {
            get
            {
                if (animator == null)
                    animator = Resources.Load<RuntimeAnimatorController>(animatorPath);

                return animator;
            }
            set => animator = value;
        }

        private bool isFlying = false;
        public bool IsFlying { get => isFlying; set => isFlying = value; }

        /// <summary>
        /// DB로 부터 초기화
        /// </summary>
        ///// <param name="id">DB 키값</param>
        //protected Unit(int id)
        //{
        //    initializeUnitFromDB(id);
        //    CurHP = MaxHP;

        //    var skillString = descriptor.skills.Split(';');

        //    //descriptor로부터 걷기 추가
        //    moveSkill = (Skill)Activator.CreateInstance(Type.GetType($"Model.Skills.{descriptor.moveSkill}"));

        //    //descriptor로부터 스킬 추가
        //    for (int i = 0; i < skillString.Length; i++)
        //    {
        //        // Debug.Log(skillString[i]);
        //        skills[i] = (Skill)Activator.CreateInstance(Type.GetType($"Model.Skills.{skillString[i]}"));
        //    }
        //}

        //private void initializeUnitFromDB(int no)
        //{
        //    var _descriptor = UnitStorage.Instance[no];
        //    if (_descriptor != null)
        //    {
        //        descriptor = _descriptor.Copy();
        //    }
        //    else
        //    {
        //        Debug.LogError($"number={no}에 해당하는 유닛이 없습니다.");
        //    }
        //}
    }
}