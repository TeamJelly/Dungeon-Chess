using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_013 : Skill
    {
        Extension_013 parsedExtension;
        public Extension_013 ParsedExtension => parsedExtension;
        public Skill_013() : base(13)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_013>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_013 : Extensionable
    {
    }
}