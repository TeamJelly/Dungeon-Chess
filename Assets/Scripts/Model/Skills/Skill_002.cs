using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    /// <summary>
    /// 스킬 이름: 마법 화살(Colored_280)
    /// </summary>
    public class Skill_002 : Skill
    {
        Extension_002 parsedExtension;
        public Extension_002 ParsedExtension => parsedExtension;
        public Skill_002() : base(2)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_002>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_002 : Extensionable
    {
    }
}