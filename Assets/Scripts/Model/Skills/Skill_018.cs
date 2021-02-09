using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_018 : Skill
    {
        private Extension_018 parsedExtension;
        public Extension_018 ParsedExtension => parsedExtension;
        public Skill_018() : base(18)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_018>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_018 : Extensionable
    {
    }
}