using Common;
using UnityEngine;

public enum Category { NULL, Party, Neutral, Friendly, Enemy, Boss};
public enum UnitClass { NULL, Monster, Warrior, Wizard, Priest, Archer };
public enum UnitAI {NULL, AI1, AI2, AI3, AI4 };

namespace Model
{
    [System.Serializable]
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
        public int move = 3;
        public int critical;

        [Header("Position")]
        public Vector2Int position;

        [Header("Hidden Status")]
        public float actionRate;

        [Header("Count Status")]
        public int moveCount;
        public int skillCount;
        public int itemCount;

        private Skill[] _skills = new Skill[4];
        public Skill[] skills { get => _skills; }
        [Header("Having")]
        //public string[] havingSkills = new string[4];
        public List<Artifact> antiques = new List<Artifact>();
        public Item[] items = new Item[2];
        public List<Effect> stateEffects = new List<Effect>();

        public Transform transform;
        // 초기화 함수
        /*private void Awake()
        {
            AwakeSkills();
        }*/

        /// <summary>
        /// DB로부터 정보를 받아 초기화 예정. (이름, unitPositoin등 위의 속성들).
        /// </summary>
        /// <param name="name"></param>
        public Unit(string name)
        {
            this.name = name;
        }
        
        public void SetUnitShape(Transform t)
        {
            this.transform = t;
            Vector2Int newPos = new Vector2Int((int)t.position.x, (int)t.position.y);
            unitPosition.Set(newPos, newPos);
            
        }
        /// <summary>
        /// 처음에 스킬 가지고 있는 것을 초기화
        /// </summary>
        /*private void AwakeSkills()
        {
            for (int i = 0; i < havingSkills.Length; i++)
            {
                if (havingSkills[i] != null)
                {
                    AddSkillComponent(havingSkills[i]);
                }
            }
        }*/

        /// <summary>
        /// (public) 스킬 등록
        /// </summary>
        /// <param name="newSkill">등록하려는 스킬 이름</param>
        /// <param name="index">슬롯 위치</param>
        public void setSkill(Skill newSkill, int index)
        {
            if (index >= skills.Length)
            {
                //TODO UI Error Message
                return;
            }
            //havingSkills[index] = newSkill;
            //Type type = Type.GetType(newSkill);
            //Skill instance = Activator.CreateInstance(Type.GetType(newSkill)) as Skill;
            
            skills[index] = newSkill;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">slot 위치</param>
        public void removeSkill(int index)
        {
            if (index >= skills.Length || skills[index] == null)
            {
                //TODO UI Error Message
                return;
            }
            //havingSkills[index] = "";
            skills[index] = null;
           // Destroy(skills[index]);
        }
    }
}

