using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_005 : Skill
    {
        Extension_005 parsedExtension;
        public Extension_005 ParsedExtension => parsedExtension;
        public Skill_005() : base(5)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_005>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_005 : Extensionable
    {
    }
}