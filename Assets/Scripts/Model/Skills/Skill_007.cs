using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_007 : Skill
    {
        Extension_007 parsedExtension;
        public Extension_007 ParsedExtension => parsedExtension;
        public Skill_007() : base(7)
        {
            if (extension != null)
            {
                parsedExtension = Common.Extension.Parse<Extension_007>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_007 : Common.Extensionable
    {
    }
}