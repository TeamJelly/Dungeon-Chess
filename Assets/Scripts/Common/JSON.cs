using UnityEngine;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// JSON 관련 유틸리티 클래스 입니다.
    /// </summary>
    public class JSON
    {
        public static string DictionaryToJsonString(Dictionary<string, object> dict)
        {
            List<string> str = new List<string>();

            foreach (var item in dict)
            {
                str.Add($"\"{item.Key}\":{item.Value}");
            }

            string jsonString = "{" + string.Join(",", str) + "}";

            return jsonString;
        }
        public static T ParseFile<T>(string path)
        {
            var jsonData = Resources.Load(path) as TextAsset;
            return JsonUtility.FromJson<T>(jsonData.text);
        }
        public static T ParseString<T>(string jsonString)
        {
            return JsonUtility.FromJson<T>(jsonString);
        }
        private JSON() { } // Script에 의해 생성될 수 없다.
    }
}