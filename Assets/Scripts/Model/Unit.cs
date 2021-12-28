using UnityEngine;
using System.Collections.Generic;
using Common;
using System;
using Model;
using Model.Skills.Move;

namespace Model
{
    /// <summary>
    /// 진영 : 유닛이 속한 진영을 결정한다. 전투중 행동방식을 결정
    /// </summary>
    public enum UnitAlliance
    {
        NULL = -1,
        Party,
        Enemy,
        Neutral,
        Friendly,
    };

    /// <summary>
    /// 종족값 : 외형과 시작 스테이터스를 결정한다.
    /// </summary>
    public enum UnitSpecies
    {
        NULL = -1,
        Human,          // 인간
        SmallBeast,     // 소형짐승
        MediumBeast,    // 중형짐승
        LargeBeast,     // 대형짐승
        Golem           // 골렘
    }

    /// <summary>
    /// 수식어 : 성장 스테이터스를 결정한다.
    /// </summary>
    public enum UnitModifier
    {
        NULL = -1,      // Weak
        Tough,          // 군인 > 강인한
        Deadly,         // 현상금 사냥꾼 > 치명적인
        Meticulous,     // 용병 > 세심한
        Righteous,       // 시민 > 옳은, 정의로운
        Quick,          // 도둑 > 재빠른
        Rich,           // 상인 > 부유한
    };

    [Serializable]
    public class Unit : Spriteable
    {
        public Unit() { }

        // 유닛 생성 시스템
        public Unit(UnitAlliance alliance, UnitSpecies species = UnitSpecies.NULL, int lv = 1)
        {
            Seed = UnityEngine.Random.Range(0, 10000);
            Alliance = alliance;

            if (species == UnitSpecies.NULL)
                Species = (UnitSpecies)(Seed % 5); // 0 ~ 3
            else
                Species = species;

            Modifier = (UnitModifier)(Seed % 6);

            Name = Data.GetRandomName(Seed);
            Sprite = Data.GetRandomSprite(Species, Seed);

            MoveSkill = Data.GetRandomSkill(Seed, species, SkillCategory.Move) as Model.Skills.Move.MoveSkill;
            Skills.Add(Data.GetRandomSkill(Seed, species, SkillCategory.Basic));

            // Skills.Add(Data.GetRandomSkill(Seed, species, SkillCategory.Intermediate));
            // Skills.Add(Data.GetRandomSkill(Seed, species, SkillCategory.Advanced));

            // 인간형 초기 스텟
            if (Species == UnitSpecies.Human)
            {
                MaxHP = 35;
                Strength = 10;
                Agility = 10;
                Mobility = 3;
                CriRate = 10;
            }
            else if (Species == UnitSpecies.SmallBeast)
            {
                MaxHP = 20;
                Strength = 12;
                Agility = 11;
                Mobility = 4;
                CriRate = 11;
            }
            else if (Species == UnitSpecies.MediumBeast)
            {
                MaxHP = 30;
                Strength = 12;
                Agility = 10;
                Mobility = 3;
                CriRate = 11;
            }
            else if (Species == UnitSpecies.LargeBeast)
            {
                MaxHP = 50;
                Strength = 15;
                Agility = 12;
                Mobility = 2;
                CriRate = 11;
            }

            Level = lv;

            CurEXP = 0;
            NextEXP = 10 * Level * (Level + 5);
        }

        public enum AnimationState { Idle, Hit, Attack, Move, Heal };

        private int Seed { get; set; }
        public string Name { get; set; }            // 이름
        private int level = 1;
        private int curEXP;                         // 현재 경험치
        private int nextEXP;                        // 다음 레벨업에 필요한 경험치 
        private int curHP;                          // 현재 체력
        private int maxHP;                          // 최대 체력
        private int armor;                          // 방어도 (추가 체력)
        private int strength;                       // 힘
        private int agility;                        // 민첩
        private int mobility;                           // 이동력
        private int criRate;                 // 치명타 율
        private float actionRate;                   // 행동가능 퍼센테이지
        private Vector2Int position;                // 위치

        // 레거시 코드
        // private Dictionary<SkillCategory, Skill> skills = new Dictionary<SkillCategory, Skill>()
        // {
        //     {SkillCategory.Move, null},
        //     {SkillCategory.Basic, null},
        //     {SkillCategory.Intermediate, null},
        //     {SkillCategory.Advanced, null}
        // };

        private MoveSkill moveSkill = new MoveSkill();
        private List<Skill> skills = new List<Skill>();
        private Dictionary<Skill, int> waitingSkills = new Dictionary<Skill, int>();
        private Dictionary<Skill, int> enhancedSkills = new Dictionary<Skill, int>();
        private List<Obtainable> belongings = new List<Obtainable>();    // 보유한 유물 및 아이템
        private List<Effect> stateEffects = new List<Effect>();  // 보유한 상태효과
        private List<Obtainable> droptems = new List<Obtainable>();
        private RuntimeAnimatorController animator; // 애니메이터
        protected string animatorPath = "";
        public AnimationState animationState = AnimationState.Idle; // 현재 애니메이션 상태
        public UnitAlliance Alliance { get; set; }  // 진영
        public UnitSpecies Species { get; set; }       // 종족
        public UnitModifier Modifier { get; set; }      // 수식어

        // protected string spritePath;
        public Sprite Sprite { get; set; }

        // 유닛 이벤트 모음
        public TimeEventListener<bool> OnBattleStart = new TimeEventListener<bool>();
        public TimeEventListener<bool> OnBattleEnd = new TimeEventListener<bool>();
        public TimeEventListener<bool> OnTurnStart = new TimeEventListener<bool>();
        public TimeEventListener<bool> OnTurnEnd = new TimeEventListener<bool>();

        public TimeEventListener<int> OnCurHP = new TimeEventListener<int>();
        public TimeEventListener<int> OnHeal = new TimeEventListener<int>();
        public TimeEventListener<int> OnDamage = new TimeEventListener<int>();
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

        public int Level
        {
            get => level;
            set
            {
                while (level < value)
                {
                    value = OnLevel.before.Invoke(value);

                    level++;

                    if (Modifier == UnitModifier.NULL)
                    {
                        MaxHP += 5;
                        Strength += 1;
                    }
                    else if (Modifier == UnitModifier.Tough)
                    {
                        MaxHP += 9;
                        Strength += 1;
                    }
                    else if (Modifier == UnitModifier.Deadly)
                    {
                        MaxHP += 1;
                        Strength += 2;
                        CriRate += 2;
                    }
                    else if (Modifier == UnitModifier.Meticulous)
                    {
                        MaxHP += 4;
                        Strength += 2;
                    }
                    else if (Modifier == UnitModifier.Righteous)
                    {
                        MaxHP += 5;
                        Strength += 1;
                        Agility = (level % 2) == 0 ? Agility + 1 : Agility;
                    }
                    else if (Modifier == UnitModifier.Quick)
                    {
                        Strength += 1;
                        Agility += 1;
                        CriRate += 2;
                    }
                    else if (Modifier == UnitModifier.Rich)
                    {
                        MaxHP += 5;
                        Strength += 1;
                        Managers.GameManager.Instance.Gold += 40;
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
        public int MaxHP
        {
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
        public int Mobility { get => mobility; set => mobility = value; } //이동 반경
        public float ActionRate { get => actionRate; set => actionRate = value; }
        public bool IsSkilled { get; set; }
        public bool IsMoved { get; set; }

        public MoveSkill MoveSkill { get => moveSkill; set => moveSkill = value; }
        public List<Skill> Skills { get => skills; set => skills = value; }

        /// <summary>
        /// 대기중인 스킬과 현재 남은 대기 턴수를 알려줍니다. 
        /// </summary>
        /// <value> 남은 대기 턴수 </value>
        public Dictionary<Skill, int> WaitingSkills { get => waitingSkills; set => waitingSkills = value; }

        /// <summary>
        /// 강화한 스킬과 스킬의 강화레벨을 알려줍니다.
        /// </summary>
        /// <value> 스킬의 레벨 </value>
        public Dictionary<Skill, int> EnhancedSkills { get => enhancedSkills; set => enhancedSkills = value; }
        public List<Obtainable> Belongings { get => belongings; set => belongings = value; }
        public List<Effect> StateEffects { get => stateEffects; set => stateEffects = value; }
        public int CriRate { get => criRate; set => criRate = value; }

        public virtual List<Obtainable> Droptems
        {
            set => droptems = value;
            get
            {
                List<Obtainable> obtainables = new List<Obtainable>();

                foreach (var item in droptems)
                    obtainables.Add(item);

                foreach (var item in Belongings)
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

        public Color Color => throw new NotImplementedException();

        public Unit_Serializable Get_Serializable()
        {
            Unit_Serializable u = new Unit_Serializable
            {
                Seed = Seed,
                Name = Name,
                level = level,
                curEXP = curEXP,
                nextEXP = nextEXP,
                curHP = curHP,
                maxHP = maxHP,
                armor = armor,
                strength = strength,
                agility = agility,
                mobility = mobility,
                criRate = criRate,
                actionRate = actionRate,
                positionX = position.x,
                positionY = position.y,
                moveSkill = Data.AllSkills.IndexOf(moveSkill),
                animatorPath = animatorPath,
                animationState = (int)animationState,
                Alliance = (int)Alliance,
                Species = (int)Species,
                Modifier = (int)Modifier,
                IsSkilled = IsSkilled,
                IsMoved = IsMoved,
                IsFlying = IsFlying,
                //ColorR = Color.r,
                //ColorG = Color.g,
                //ColorB = Color.b
            };

            foreach(Skill s in skills)
            {
                u.skills.Add(Data.AllSkills.IndexOf(s));
            }
            foreach (Skill s in waitingSkills.Keys)
            {
                u.waitingSkills.Add(Data.AllSkills.IndexOf(s), waitingSkills[s]);
            }
            foreach (Skill s in enhancedSkills.Keys)
            {
                u.enhancedSkills.Add(Data.AllSkills.IndexOf(s), enhancedSkills[s]);
            }
            for (int i = 0; i < stateEffects.Count; i++)
            {
                u.stateEffects.Add(stateEffects[i].ToString());
                u.stateEffects_count.Add(stateEffects[i].TurnCount);
            }

            foreach(Obtainable o in belongings)
            {
                u.belongings.Add(o.ToString());
            }
            return u;
        }

        public Unit(Unit_Serializable u)
        {
            Set_From_Serializable(u);
        }
        public void Set_From_Serializable(Unit_Serializable u)
        {
            Seed = u.Seed;
            Name = u.Name;
            level = u.level;
            curEXP = u.curEXP;
            nextEXP = u.nextEXP;
            curHP = u.curHP;
            maxHP = u.maxHP;
            armor = u.armor;
            strength = u.strength;
            agility = u.agility;
            mobility = u.mobility;
            criRate = u.criRate;
            actionRate = u.actionRate;
            position.x = u.positionX;
            position.y = u.positionY;
            moveSkill = (MoveSkill)Data.AllSkills[u.moveSkill];
            animatorPath = u.animatorPath;
            animationState = (AnimationState)u.animationState;
            Alliance = (UnitAlliance)u.Alliance;
            Species = (UnitSpecies)u.Species;
            Modifier = (UnitModifier)u.Modifier;
            //Sprite = u.1;
            IsSkilled = u.IsSkilled;
            IsMoved = u.IsMoved;
            isFlying = u.IsFlying;

            skills.Clear();
            waitingSkills.Clear();
            enhancedSkills.Clear();
            stateEffects.Clear();
            belongings.Clear();

            foreach (Skill s in skills)
            {
                Common.Command.RemoveSkill(this, s);
            }
            foreach (Effect e in stateEffects)
            {
                Common.Command.RemoveEffect(this, e);
            }
            foreach (Obtainable o in belongings)
            {
                Common.Command.RemoveArtifact(this, (Artifact)o);
            }


            foreach (int s in u.skills)
            {
                Common.Command.AddSkill(this, Data.AllSkills[s]);
                //skills.Add(Data.AllSkills[s]);
            }
            foreach (int s in u.waitingSkills.Keys)
            {
                waitingSkills.Add(Data.AllSkills[s], u.waitingSkills[s]);
            }
            foreach (int s in u.enhancedSkills.Keys)
            {
                enhancedSkills.Add(Data.AllSkills[s], u.enhancedSkills[s]);
            }

            for (int i = 0; i < u.stateEffects.Count; i++)
            {
                if(u.stateEffects_count[i] > 0)
                    Common.Command.AddEffect(this, (Effect)Activator.CreateInstance(Type.GetType(u.stateEffects[i]),this, u.stateEffects_count[i]));
                
                else Common.Command.AddEffect(this, (Effect)Activator.CreateInstance(Type.GetType(u.stateEffects[i]), this));
            }

            foreach (string o in u.belongings)
            {
                Common.Command.AddArtifact(this, (Artifact)Activator.CreateInstance(Type.GetType(o)));
            }
            Sprite = Data.GetRandomSprite(Species, Seed);

        }
    }

    public class Unit_Serializable
    {
        public int Seed;
        public string Name;                        // 이름
        public int level = 1;
        public int curEXP;                         // 현재 경험치
        public int nextEXP;                        // 다음 레벨업에 필요한 경험치 
        public int curHP;                          // 현재 체력
        public int maxHP;                          // 최대 체력
        public int armor;                          // 방어도 (추가 체력)
        public int strength;                       // 힘
        public int agility;                        // 민첩
        public int mobility;                           // 이동력
        public int criRate;                 // 치명타 율
        public float actionRate;                   // 행동가능 퍼센테이지
        public int positionX;                // 위치X
        public int positionY;                // 위치Y
        public int moveSkill;
        public List<int> skills = new List<int>();                  
        public Dictionary<int, int> waitingSkills = new Dictionary<int, int>();
        public Dictionary<int, int> enhancedSkills = new Dictionary<int, int>();
        public List<string> stateEffects = new List<string>();  // 보유한 상태효과
        public List<int> stateEffects_count = new List<int>();  // 보유한 상태효과
        public List<string> belongings = new List<string>();  // 보유한 유물
        public string animatorPath = "";
        public int animationState;
        public int Alliance;
        public int Species;
        public int Modifier;
        public bool IsSkilled;
        public bool IsMoved;
        public bool IsFlying;
        public float ColorR;
        public float ColorG;
        public float ColorB;

        public Unit_Serializable() { }
    }
}