using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_015 : Skill
    {
        private Extension_015 parsedExtension;
        public Extension_015 ParsedExtension => parsedExtension;
        public Skill_015() : base(15)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_015>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_015 : Extensionable
    {
    }
}