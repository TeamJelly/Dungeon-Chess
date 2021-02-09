using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_001 : Skill
    {
        Extension_001 parsedExtension;
        public Extension_001 ParsedExtension => parsedExtension;
        public Skill_001() : base(1)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_001>(extension);
            }
        }
    }

        [System.Serializable]
    public class Extension_001 : Extensionable
    {
    }
}