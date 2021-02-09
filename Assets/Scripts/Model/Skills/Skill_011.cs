using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_011 : Skill
    {
        Extension_011 parsedExtension;
        public Extension_011 ParsedExtension => parsedExtension;
        public Skill_011() : base(11)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_011>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_011 : Extensionable
    {
    }
}