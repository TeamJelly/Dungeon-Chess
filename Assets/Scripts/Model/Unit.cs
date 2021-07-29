using UnityEngine;
using System.Collections.Generic;
using Common;
using System;

namespace Model
{
    public enum UnitAlliance { NULL, Party, Neutral, Friendly, Enemy };

    public enum Species {
        
    }

    public enum Modifier {
        NULL,
        Monster,
        Soldier,        // 군인 > 강인한
        BountyHunter,   // 현상금 사냥꾼 > 치명적인
        Mercenary,      // 용병 > 계산적인, 치밀한
        Villeager,      // 시민 > 평범한
        Thief,          // 도둑 > 재빠른
        Merchant,       // 상인 > 부유한
    };

    [Serializable]
    public class Unit : Spriteable
    {
        public Unit() { }

        // 유닛 생성 시스템
        public Unit(UnitAlliance alliance, int lv = 1)
        {
            seed = UnityEngine.Random.Range(0, 10000);

            this.Alliance = alliance;
            
            if (Alliance == UnitAlliance.Party)
                Class = (Modifier) UnityEngine.Random.Range(2,8);
            else
                Class = Modifier.Monster;
            
            Name = Data.GetRandomName(seed);
            Sprite = Data.GetRandomSprite(seed);
            MoveSkill = new Model.Skills.S100_Walk();
            Skills[0] = Data.GetRandomSkill(seed);
            Skills[1] = Data.GetRandomSkill(seed);
            Skills[2] = Data.GetRandomSkill(seed);

            // 군인 초기스텟
            if (Class == Modifier.Soldier)
            {
                MaxHP = 40;
                Strength = 7;
                Agility = 10;
                Move = 3;
                CriticalRate = 10;
            }
            // 현상금 사냥꾼 초기스텟
            else if (Class == Modifier.BountyHunter)
            {
                MaxHP = 35;
                Strength = 4;
                Agility = 10;
                Move = 3;
                CriticalRate = 20;
            }
            // 용병 초기스텟
            else if (Class == Modifier.Mercenary)
            {
                MaxHP = 30;
                Strength = 10;
                Agility = 10;
                Move = 3;
                CriticalRate = 10;
            }
            // 시민 초기스텟
            else if (Class == Modifier.Villeager)
            {
                MaxHP = 45;
                Strength = 6;
                Agility = 9;
                Move = 3;
                CriticalRate = 10;
            }
            // 도둑 초기스텟
            else if (Class == Modifier.Thief)
            {
                MaxHP = 30;
                Strength = 10;
                Agility = 10;
                Move = 4;
                CriticalRate = 10;
            }
            // 상인 초기스텟
            else if (Class == Modifier.Merchant)
            {
                MaxHP = 20;
                Strength = 10;
                Agility = 10;
                Move = 3;
                CriticalRate = 10;
            }
            // 그 이외 초기스텟
            else
            {
                MaxHP = 30;
                Strength = 10;
                Agility = 10;
                Move = 3;
                CriticalRate = 10;
            }

            Level = lv;
            
            CurEXP = 0;
            NextEXP = 10 * Level * (Level + 5);
        }

        public enum AnimationState { Idle, Hit, Attack, Move, Heal };

        private int seed;
        public string Name { get; set; }            // 이름
        private int level = 1;
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
        private List<Artifact> artifacts = new List<Artifact>();    // 보유한 유물
        private List<Effect> stateEffects = new List<Effect>();  // 보유한 상태효과
        private List<Obtainable> droptems = new List<Obtainable>();

        private RuntimeAnimatorController animator; // 애니메이터

        protected String animatorPath = "";
        public AnimationState animationState = AnimationState.Idle; // 현재 애니메이션 상태
        public UnitAlliance Alliance { get; set; }  // 진영
        public Modifier Class { get; set; }        // 직업

        // protected string spritePath;
        public Sprite Sprite { get; set;}

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

        public int Level {
            get => level;
            set
            {
                while (level < value)
                {
                    value = OnLevel.before.Invoke(value);

                    level++;
                    if (Class == Modifier.Soldier)
                    {
                        MaxHP += 9;
                        Strength += 1;
                    }
                    else if (Class == Modifier.BountyHunter)
                    {
                        MaxHP += 1;
                        Strength += 2;
                        CriticalRate += 2;
                    }
                    else if (Class == Modifier.Mercenary)
                    {
                        MaxHP += 4;
                        Strength += 2;
                    }
                    else if (Class == Modifier.Villeager)
                    {
                        MaxHP += 5;
                        Strength += 1;
                        Agility = (level % 2) == 0 ? Agility + 1 : Agility;
                    }
                    else if (Class == Modifier.Thief)
                    {
                        Strength += 1;
                        Agility += 1;
                        CriticalRate += 2;
                    }
                    else if (Class == Modifier.Merchant)
                    {
                        MaxHP += 5;
                        Strength += 1;
                        Managers.GameManager.Instance.Gold += 40;
                    }
                    else
                    {
                        MaxHP += 5;
                        Strength += 1;
                    }

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