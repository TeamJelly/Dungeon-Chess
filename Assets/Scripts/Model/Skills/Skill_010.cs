using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_010 : Skill
    {
        Extension_010 parsedExtension;
        public Extension_010 ParsedExtension => parsedExtension;
        public Skill_010() : base(10)
        {
            if (extension != null)
            {
                parsedExtension = Common.Extension.Parse<Extension_010>(extension);
            }
        }
    }

        [System.Serializable]
    public class Extension_010 : Common.Extensionable
    {
    }
}