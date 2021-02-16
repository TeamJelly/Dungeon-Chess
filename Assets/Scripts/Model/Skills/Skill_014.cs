using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_014 : Skill
    {
        private Extension_014 parsedExtension;
        public Extension_014 ParsedExtension => parsedExtension;
        public Skill_014() : base(14)
        {
            if (extension != null)
            {
                parsedExtension = Common.Extension.Parse<Extension_014>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_014 : Common.Extensionable
    {
    }
}