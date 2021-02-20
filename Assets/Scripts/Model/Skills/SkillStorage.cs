using Common.DB;

namespace Model.Skills
{
    public class SkillStorage : Storage<int, Skill> 
    {
        private static SkillStorage instance = new SkillStorage();
        public static SkillStorage Instance => instance;
        private SkillStorage() { }
    }
}