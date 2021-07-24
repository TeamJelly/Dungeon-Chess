using UnityEngine;
using System.Collections.Generic;
using Common;
using System;

namespace Model
{
    public enum UnitAlliance { NULL, Party, Neutral, Friendly, Enemy };
    public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };

    [Serializable]
    public class Unit : Spriteable
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
        // private Skill passiveSkill;                 // 패시브 스킬
        private Skill[] skills = new Skill[3];      // 4가지 스킬

        // 배울수 있는 스킬
        private Skill[] learnableSkill = new Skill[8];

        private List<Artifact> artifacts = new List<Artifact>();    // 보유한 유물
        private List<Effect> stateEffects = new List<Effect>();  // 보유한 상태효과
        private List<Obtainable> droptems = new List<Obtainable>();

        private RuntimeAnimatorController animator; // 애니메이터

        protected String animatorPath = "";
        public AnimationState animationState = AnimationState.Idle; // 현재 애니메이션 상태
        public UnitAlliance Alliance { get; set; }  // 진영
        public UnitClass Class { get; set; }        // 직업

        protected string spritePath;
        public Sprite Sprite { get => Common.Data.LoadSprite(spritePath); }

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
        public TimeEventListener<Vector2Int> OnMove = new TimeEventListener<Vector2Int>();


        // HP가 변했을때 효과 추가 필요
        public int CurHP
        {
            get => curHP;
            set
            {
                if (curHP != value)
                {
                    value = OnCurHP.before.Invoke(value);
                    curHP = value;
                    OnCurHP.after.Invoke(value);
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
                    value = OnArmor.before.Invoke(value);
                    armor = value;
                    OnArmor.after.Invoke(value);
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
                    value = OnLevel.before.Invoke(value);
                    level = value;
                    OnLevel.after.Invoke(value);
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
                    value = OnCurEXP.before.Invoke(value);
                    curEXP = value;
                    OnCurEXP.after.Invoke(value);
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
                        value = OnPosition.before.Invoke(value);
                        position = value;
                        OnPosition.after.Invoke(value);
                    }
                }
                else
                    Debug.LogError("유닛을 이곳으로 이동할 수 없습니다.");
            }
        }

        public int NextEXP { get => nextEXP; set => nextEXP = value; }
        public int Strength { get => strength; set => strength = value; }
        public int MaxHP {
            get => maxHP; 
            set
            {
                // maxHP가 줄어들어 curHP보다 줄어드는 경우
                if (curHP > value)
                {
                    // MaxHP와 CurHP를 value값으로 맞춰줍니다.
                    curHP = value;
                    maxHP = value;
                }
                // maxHP가 증가하는 경우 curHP또한 키웁니다.
                else if (maxHP < value)
                {
                    int increasingValue = value - maxHP;
                    maxHP = value;
                    curHP = value;
                }
                // 그 이외의 경우엔 그냥 maxHP만 value로 조정합니다.
                else
                    maxHP = value;
            }
        }
        public int Agility { get => agility; set => agility = value; }
        public int Move { get => move; set => move = value; } //이동 반경
        public float ActionRate { get => actionRate; set => actionRate = value; }
        public int SkillCount { get => skillCount; set => skillCount = value; }
        public Skill MoveSkill { get => moveSkill; set => moveSkill = value; }
        // public Skill PassiveSkill { get => passiveSkill; set => passiveSkill = value; }
        public Skill[] Skills { get => skills; set => skills = value; }
        public List<Artifact> Artifacts { get => artifacts; set => artifacts = value; }
        public List<Effect> StateEffects { get => stateEffects; set => stateEffects = value; }
        public int MoveCount { get => moveCount; set => moveCount = value; }
        public int CriticalRate { get => criticalRate; set => criticalRate = value; }
        
        public virtual List<Obtainable> Droptems {
            set => droptems = value;
            get 
            {
                List<Obtainable> obtainables = new List<Obtainable>();

                foreach (var item in droptems)
                    obtainables.Add(item);

                foreach (var item in Artifacts)
                    obtainables.Add(item);

                return obtainables;
            }
        }

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
    }
}