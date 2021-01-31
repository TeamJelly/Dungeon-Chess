using UnityEngine;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// JSON 관련 유틸리티 클래스 입니다.
    /// </summary>
    public class JSON
    {
        public static TextAsset DictionaryToTextAsset(Dictionary<string, object> dict)
        {
            List<string> str = new List<string>();

            

            foreach (var item in dict)
            {
                str.Add(string.Format("\"{0}\":{1}", item.Key, item.Value));
            }

            TextAsset asset = new TextAsset("{" + string.Join(",", str) + "}");
            Debug.Log(asset.text);
            return asset;
        }
        public static T ParseFile<T>(string path)
        {
            var jsonData = Resources.Load(path) as TextAsset;
            return JsonUtility.FromJson<T>(jsonData.text);
        }
        public static T ParseAsset<T>(TextAsset jsonAsset)
        {
            return JsonUtility.FromJson<T>(jsonAsset.text);
        }
        private JSON() { } // Script에 의해 생성될 수 없다.
    }
}