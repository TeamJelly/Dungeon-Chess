using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace View
{
    public class SystemMessageView : MonoBehaviour
    {
        public static SystemMessageView instance;

        public GameObject panel;
        public TextMeshProUGUI text; 

        private void Awake() {
            instance = this;
        }

        public static void SetMessage(string text)
        {
            instance.StopAllCoroutines();
            instance.text.alpha = 1;
            instance.text.text = text;
            Common.UIEffect.FadeInPanel(instance.panel);
        }

        public static void HideMessage()
        {
            instance.text.alpha = 0;
            Common.UIEffect.FadeOutPanel(instance.panel);
        }

        public static void ReserveMessage(string text)
        {            
            instance.panel.gameObject.SetActive(true);
            instance.text.text = text;
            instance.StartCoroutine(Common.UIEffect.FadeOutCoroutine(instance.panel, instance.panel.GetComponent<CanvasGroup>(), 2, 1));
        }
    }
}