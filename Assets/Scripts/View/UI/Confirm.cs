using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace View.UI
{

    public class Confirm : MonoBehaviour
    {
        public GameObject ConfirmUI;
        static Confirm instance;
        public TextMeshProUGUI title;
        public Button yesButton;
        public Button noButton;
        private void Awake()
        {
            instance = this;
        }

        public static void Enable(string title, UnityAction Confirm, UnityAction Discard)
        {
            instance.title.text = title;

            instance.yesButton.onClick.RemoveAllListeners();
            instance.noButton.onClick.RemoveAllListeners();

            instance.yesButton.onClick.AddListener(Confirm);
            instance.noButton.onClick.AddListener(Discard);

            Common.UIEffect.FadeInPanel(instance.ConfirmUI);
        }

        public static void Disable()
        {
            Common.UIEffect.FadeOutPanel(instance.ConfirmUI);
        }
    }

}
