using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR

[InitializeOnLoad]
public class AndroidKeystoreLoader
{
	static AndroidKeystoreLoader()
	{
		string keystorePass, keyaliasName, keyaliasPass;
		
		keystorePass = KeystoreHelper.ReadPrefs(KeystoreHelper.KEYSTOREPASS);
		keyaliasName = KeystoreHelper.ReadPrefs(KeystoreHelper.KEYALIASNAME);
		keyaliasPass = KeystoreHelper.ReadPrefs(KeystoreHelper.KEYALIASPASS);
		
		PlayerSettings.Android.keystorePass = keystorePass;
		PlayerSettings.Android.keyaliasName = keyaliasName;
		PlayerSettings.Android.keyaliasPass = keyaliasPass;		
	}
}

#endif