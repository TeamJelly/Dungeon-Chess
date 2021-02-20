using Common.DB;

namespace Model.Skills
{
    public class SkillStorage : Storage<int, SkillDescriptor> 
    {
        private static SkillStorage instance = new SkillStorage();
        public static SkillStorage Instance => instance;
        private SkillStorage(): base("skill_table", "number") { }
    }
    [System.Serializable]
    public class SkillDescriptor : Copyable<SkillDescriptor>
    {
        public int number;
        public string name = "";
        public UnitClass unitClass;
        public Grade grade;
        public int maxLevel;
        public string spritePath;
        public string description;
        public float criticalRate;
        public int reuseTime;
        public Skill.Type type;
        public Skill.Target target;
        public string APSchema;
        public string RPSchema;
        public string extension;
    } 
}