using UnityEngine;

namespace Common
{
    /// <summary>
    /// JSON 관련 유틸리티 클래스 입니다.
    /// </summary>
    public class JSON
    {
        public static T ParseFile<T>(string path)
        {
            var jsonData = Resources.Load(path) as TextAsset;
            return JsonUtility.FromJson<T>(jsonData.ToString());
        }
        public static T ParseAsset<T>(TextAsset jsonAsset)
        {
            return JsonUtility.FromJson<T>(jsonAsset.ToString());
        }
        private JSON() { } // Script에 의해 생성될 수 없다.
    }
}