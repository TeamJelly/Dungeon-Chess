using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_020 : Skill
    {
        private Extension_020 parsedExtension;
        public Extension_020 ParsedExtension => parsedExtension;
        public Skill_020() : base(20)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_020>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_020 : Extensionable
    {
    }
}