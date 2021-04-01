using Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class OptionUI : MonoBehaviour
    {
        public GameObject panel;

        public Slider slider;
        // Start is called before the first frame update
        void Start()
        {
            slider.maxValue = 1;
            slider.minValue = 0;
            if (BGMUI.instance == null) return;
            slider.value = BGMUI.instance.GetAudioVolume();
            slider.onValueChanged.AddListener(BGMUI.instance.SetAudioVolume);
        }

        public void Enable()
        {
            UIEffect.FadeInPanel(panel);
        }
        public void Disable()
        {
            UIEffect.FadeOutPanel(panel);
        }
        public void SetWindowModeScreen()
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }

        public void SetFullModeScreen()
        {
            Screen.SetResolution(1920, 1080,
                FullScreenMode.FullScreenWindow);
        }


    }
}