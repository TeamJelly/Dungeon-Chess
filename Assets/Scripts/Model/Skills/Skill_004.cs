using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_004 : Skill
    {
        Extension_004 parsedExtension;
        public Extension_004 ParsedExtension => parsedExtension;
        public Skill_004() : base(4)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_004>(extension);
            }
        }
    }

        [System.Serializable]
    public class Extension_004 : Extensionable
    {
    }
}