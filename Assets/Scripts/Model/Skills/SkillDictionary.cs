using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Skills
{
    public class SkillDictionary
    {
        private Dictionary<int, Skill> dict = new Dictionary<int, Skill>();
        private static SkillDictionary instance = new SkillDictionary();
        public static SkillDictionary Instance => instance;
        private SkillDictionary() { }
        public Skill this[int key]
        {
            get
            {
                try
                {
                    return dict[key];
                }
                catch (KeyNotFoundException)
                {
                    // Debug.LogError($"{key}에 해당하는 스킬을 찾을 수 없습니다.");
                    return null;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return null;
                }
            }
            set => dict[key] = value;
        }
    }
}