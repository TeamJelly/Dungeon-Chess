using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class Skill_021 : Skill
    {
        private Extension_021 parsedExtension;
        public Extension_021 ParsedExtension => parsedExtension;
        public Skill_021() : base(21)
        {
            if (extension != null)
            {
                parsedExtension = ParseExtension<Extension_021>(extension);
            }
        }
    }

    [System.Serializable]
    public class Extension_021 : Extensionable
    {
    }
}