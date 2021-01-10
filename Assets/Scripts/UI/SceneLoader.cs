using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//추후 수정 예정
public class SceneLoader : MonoBehaviour
{
    public static void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
