using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Text;
using System.Reflection;

class KeystoreHelper : EditorWindow {
	
	public const string KEYSTOREPASS = "AKS_keystorePass_";
	public const string KEYALIASNAME = "AKS_keyaliasName_";
	public const string KEYALIASPASS = "AKS_keyaliasPass_";
	
	private string keystorePass = "";
	private string keyaliasName = "";
	private string keyaliasPass = "";
	
	[MenuItem ("Window/Keystore Helper")]	
	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(KeystoreHelper));
	}
	
	void OnEnable() {
		keystorePass = ReadPrefs(KEYSTOREPASS);
		keyaliasName = ReadPrefs(KEYALIASNAME);
		keyaliasPass = ReadPrefs(KEYALIASPASS);
		
		PlayerSettings.Android.keystorePass = keystorePass;
		PlayerSettings.Android.keyaliasName = keyaliasName;
		PlayerSettings.Android.keyaliasPass = keyaliasPass;		
	}
	
	void OnDisable() {
		WritePrefs(KEYSTOREPASS, keystorePass);
		WritePrefs(KEYALIASNAME, keyaliasName);
		WritePrefs(KEYALIASPASS, keyaliasPass);
		
		PlayerSettings.Android.keystorePass = keystorePass;
		PlayerSettings.Android.keyaliasName = keyaliasName;
		PlayerSettings.Android.keyaliasPass = keyaliasPass;		
	}
	
	public static string ReadPrefs(string _key) {
		string codeBase = Assembly.GetExecutingAssembly().CodeBase;
		string key = _key+Md5Sum(codeBase);
		return Decode(EditorPrefs.GetString(key, ""));
	}
	
	public static void WritePrefs(string _key, string value) {
		string codeBase = Assembly.GetExecutingAssembly().CodeBase;
		string key = _key + Md5Sum(codeBase);
		EditorPrefs.SetString(key, Encode(value));
	}
	
	void OnGUI () {
		GUILayout.Label ("Android Keystore", EditorStyles.boldLabel);
		keystorePass = EditorGUILayout.PasswordField ("Keystore Password", keystorePass);
		keyaliasName = EditorGUILayout.TextField ("Key Alias Name", keyaliasName);
		keyaliasPass = EditorGUILayout.PasswordField ("Key Alias Password", keyaliasPass);
	}
	
	private static string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
		
		return hashString.PadLeft(32, '0');
	}
	
	private static string Encode(string inputText) {
		byte[] bytesToEncode = Encoding.UTF8.GetBytes (inputText);
		return Convert.ToBase64String (bytesToEncode);		
	}
	
	private static string Decode(string encodedText) {
		byte[] decodedBytes = Convert.FromBase64String (encodedText);
		return Encoding.UTF8.GetString (decodedBytes);		
	}	
}