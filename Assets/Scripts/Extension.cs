using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public abstract class Extensionable { }

    public class Extension
    {
        /// <summary>
        /// 확장 스탯을 파싱합니다.
        /// </summary>
        /// <typeparam name="T">확장 클래스</typeparam>
        /// <param name="extension">확장용 스키마를 넣습니다</param>
        /// <returns>확장 클래스</returns>
        public static E Parse<E>(string extension) where E : Common.Extensionable
        {
            string[] stats = extension.Split(';');
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (var stat in stats)
            {
                var stat_split = stat.Split('=');
                var name = stat_split[0];
                var value = stat_split[1];
                dict.Add(name, value);
            }

            string jsonString = JSON.DictionaryToJsonString(dict);
            return JSON.ParseString<E>(jsonString);
        }
    }
}