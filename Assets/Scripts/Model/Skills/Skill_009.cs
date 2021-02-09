using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_009 : Skill
    {
        Extension_009 parsedExtension;
        public Extension_009 ParsedExtension => parsedExtension;
        public Skill_009() : base(9)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_009>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_009 : Extensionable
    {
    }
}