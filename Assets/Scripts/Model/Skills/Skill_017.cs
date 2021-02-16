using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_017 : Skill
    {
        private Extension_017 parsedExtension;
        public Extension_017 ParsedExtension => parsedExtension;
        public Skill_017() : base(17)
        {
            if (extension != null)
            {
                parsedExtension = Common.Extension.Parse<Extension_017>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_017 : Common.Extensionable
    {
        public float maxDamageUp;
        public float minDamageUp;
        public float maxDamage;
        public float minDamage;
    }
}