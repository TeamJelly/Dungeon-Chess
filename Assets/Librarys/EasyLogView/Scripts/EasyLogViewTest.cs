using UnityEngine;

namespace mikinel.easylogview
{
    public class EasyLogViewTest : MonoBehaviour{
		public void LogTest(){
			Debug.Log($"This is test log : { Random.Range(0, 999)}");
		}

		public void WarningLogTest(){
			Debug.LogWarning($"This is test log : { Random.Range(0, 999)}");
		}

		public void ErrorLogTest(){
			Debug.LogError($"This is test log \n : { Random.Range(0, 999)}");
		}
	}
}
