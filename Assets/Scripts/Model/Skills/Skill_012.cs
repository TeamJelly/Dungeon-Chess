using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_012 : Skill
    {
        Extension_012 parsedExtension;
        public Extension_012 ParsedExtension => parsedExtension;
        public Skill_012() : base(12)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_012>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_012 : Extensionable
    {
    }
}