using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_006 : Skill
    {
        Extension_006 parsedExtension;
        public Extension_006 ParsedExtension => parsedExtension;
        public Skill_006() : base(6)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_006>(extension);
            }
        }
    }

        [System.Serializable]
    public class Extension_006 : Extensionable
    {
    }
}