using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMUI : MonoBehaviour
{
    AudioSource audioSource;

    public static BGMUI instance;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        
    }
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]

    public void SetAudioVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public float GetAudioVolume()
    {
        return audioSource.volume;
    }
}
