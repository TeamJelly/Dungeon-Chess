using System.Collections;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_003 : Skill
    {
        Extension_003 parsedExtension;
        public Extension_003 ParsedExtension => parsedExtension;
        public Skill_003() : base(3)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_003>(extension);
            }
        }
    }
    [System.Serializable]
    public class Extension_003 : Extensionable
    {
    }
}