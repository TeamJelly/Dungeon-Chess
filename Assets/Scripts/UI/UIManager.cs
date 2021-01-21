using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIManager: MonoBehaviour
{
    public static UIManager instance;
    public TMP_FontAsset TMP_FontAsset;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}